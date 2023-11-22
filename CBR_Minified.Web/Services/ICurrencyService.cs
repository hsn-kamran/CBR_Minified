using CBR_Minified.Domain.Models;

namespace CBR_Minified.Web.Services;

public interface ICurrencyService
{
    Task<ValCurs> GetCoursesByDate(DateTime dateTime);
}
