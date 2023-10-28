using CBR_Minified;
using CBR_Minified.DbModels;
using CBR_Minified.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlTypes;
using System.Text;
using System.Xml.Serialization;


InitializeEnviroment(); 

var mapper = MapperConfig.InitializeAutomapper();

var options = new DbContextOptionsBuilder<CbrDbContext>()
    .UseNpgsql(ConnectionHelper.GetConnectionString())
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

        var lastCourse = context.CurrencyCourses.OrderByDescending(c => c.Date).FirstOrDefault();

        DateTime start, end;
        start = end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        // таблица пуста
        // заполняем за месяц
        if (lastCourse is null)
            start = end - TimeSpan.FromDays(30);
        else  // иначе только за сегодняшний день
        {
            DateTime lastCourseDate = lastCourse.Date;
            start = new DateTime(lastCourseDate.Year, lastCourseDate.Month, lastCourseDate.Day + 1);
        }

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