using AutoMapper;
using CBR_Minified.Domain.DbModels;

namespace CBR_Minified.Web.Services;

public class SaveToStorageService : ISaveToStorageService
{
    private readonly CbrDbContext _context;
    private readonly ICurrencyService _getCurrencyService;
    private readonly IMapper _mapper;

    public SaveToStorageService(ICurrencyService getCurrencyService, IMapper mapper, CbrDbContext context)
    {
        _context = context;
        _getCurrencyService = getCurrencyService;
        _mapper = mapper;
    }

    public async Task Save()
    {
        try
        {
            var lastCourse = _context.CurrencyCourses.OrderByDescending(c => c.Date).FirstOrDefault();

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

            List<CurrencyCourse> currencyCourses = new List<CurrencyCourse>();

            for (DateTime date = start; date <= end; date += TimeSpan.FromDays(1))
            {
                var valCrs = await _getCurrencyService.GetCoursesByDate(date);

                // если выходной
                if (DateTime.Parse(valCrs.Date).Day != date.Day)
                    continue;

                var mappedCurrencyCourses = _mapper.Map<List<CurrencyCourse>>(valCrs.Valute);
                mappedCurrencyCourses.ForEach(cc => cc.Date = DateTime.Parse(valCrs.Date));
                currencyCourses.AddRange(mappedCurrencyCourses);
            }

            await _context.CurrencyCourses.AddRangeAsync(currencyCourses);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
