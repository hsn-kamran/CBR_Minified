using Microsoft.EntityFrameworkCore;

namespace CBR_Minified.Test
{
    /// <summary>
    /// Интеграционный тест с подменой бд в отдельном контейнере
    /// </summary>
    public class SaveToStorageServiceTest : BaseIntegrationTest
    {
        public SaveToStorageServiceTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            _enviromentService.Initialize();
        }

        [Fact]
        public async Task MockData_ShouldBeAdded_ToDb()
        {
            // Arrange
            await _dbContext.Database.MigrateAsync(); // применяем миграции чтобы создалась таблица в бд

            // Act   
            await _saveToStorageService.Save(); // по дефолту база всегда пуста
            var savedCurrencies = await _dbContext.CurrencyCourses.ToListAsync();

            // Assert
            Assert.NotEmpty(savedCurrencies);  // должны получить набор курсов валют за месяц
        }
    }
}