using CBR_Minified.Domain.Models;
using System.Xml.Serialization;

namespace CBR_Minified.Web.Services
{
    public class GetCurrencyService : ICurrencyService, IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<ValCurs> GetCoursesByDate(DateTime dateTime)
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

        public void Dispose() => _httpClient.Dispose();
    }   
}
