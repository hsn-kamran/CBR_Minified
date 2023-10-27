using CBR_Minified;
using CBR_Minified.DbModels;
using CBR_Minified.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Xml.Serialization;


InitializeEnviroment(); 

var mapper = MapperConfig.InitializeAutomapper();

var options = new DbContextOptionsBuilder<CbrDbContext>()
    .UseNpgsql(GetConnectionString())
    .Options;

await FillDatabase();





void InitializeEnviroment()
{
    // для решения конфликта поля DateTime с колонкой типа timestamp with timezone в Postgres
    // https://ru.stackoverflow.com/questions/1416392/Ошибка-cannot-write-datetime-with-kind-local-to-postgresql-type-timestamp-with
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    // https://stackoverflow.com/questions/33579661/encoding-getencoding-cant-work-in-uwp-app
    // для работы с кириллицей в xml
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    Encoding.GetEncoding("windows-1251");
}

string GetConnectionString()
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json", true, true)
        .Build();

    string cs = configuration.GetSection("ConnectionStrings:CBR_Postgres_Connection").Value
        ?? throw new ArgumentNullException(nameof(configuration));

    return cs;
}

async Task<ValCurs> GetValutesCoursesByDate(DateTime dateTime)
{   
    string date_req = dateTime.ToString("dd/MM/yyyy");
    string url = $"http://www.cbr.ru/scripts/XML_daily.asp?date_req={date_req}";

    ValCurs valCurs = new ValCurs();

    using var _httpClient = new HttpClient();
    var response = await _httpClient.GetAsync(url);

    var xmlSerializer = new XmlSerializer(typeof(ValCurs));
    string sXml = await response.Content.ReadAsStringAsync();

    using var reader = new StringReader(sXml);
    valCurs = xmlSerializer.Deserialize(reader) as ValCurs;

    return valCurs;
}


async Task FillDatabase()
{
    try
    {
        using var context = new CbrDbContext(options);

        DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        DateTime end = start;

        // в таблице актуальные данные
        if (await context.CurrencyCourses.AnyAsync()
        && context.CurrencyCourses.FirstOrDefault(c => c.Date.Year == end.Year && c.Date.Month == end.Month && c.Date.Day == end.Day) is not null)
            return;

        // если таблица пуста, значит приложение ни разу не запускалось 
        // и нужно заполнить бд данными как за предыдущий месяц, так и за сегодняшний день        
        // иначе просто заполняем за сегодняшний день
        if (!await context.CurrencyCourses.AnyAsync())
            start = end - TimeSpan.FromDays(30);

        var currencyCourses = new List<CurrencyCourse>();

        for (DateTime date = start; date <= end; date += TimeSpan.FromDays(1))
        {
            var valCrs = await GetValutesCoursesByDate(date);

            // если выходной
            if (DateTime.Parse(valCrs.Date).Day != date.Day)
                continue;

            var mappedCurrencyCourses = mapper.Map<List<CurrencyCourse>>(valCrs.Valute);
            mappedCurrencyCourses.ForEach(cc => cc.Date = DateTime.Parse(valCrs.Date));

            currencyCourses.AddRange(mappedCurrencyCourses);
        }

        await context.CurrencyCourses.AddRangeAsync(currencyCourses);
        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }  
}