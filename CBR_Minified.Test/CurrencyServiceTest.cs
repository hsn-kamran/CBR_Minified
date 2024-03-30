using CBR_Minified.Web.Services;

namespace CBR_Minified.Test
{
    /// <summary>
    /// Проверяем доступность и ответ эндпоинта
    /// </summary>
    public class CurrencyServiceTest
    {
        private readonly HttpClient _httpClient;
        private readonly ICurrencyService _currencyService;
        
        public CurrencyServiceTest()
        {
            EnviromentService enviromentService = new EnviromentService();
            enviromentService.Initialize();

            _currencyService = new GetCurrencyService();
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task Endpoint_ShouldReturn_SuccessStatusCodeAndNotEmptyResult()
        {
            // Arrange
            string date_req = DateTime.UtcNow.ToString("dd/MM/yyyy");
            string endpointUrl = $"http://www.cbr.ru/scripts/XML_daily.asp?date_req={date_req}";

            // Act
            var response = await _httpClient.GetAsync(endpointUrl);
            var res = await _currencyService.GetCoursesByDate(DateTime.Now);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotEmpty(res.Valute);
        }
    }
}
