using CBR_Minified.Web;
using CBR_Minified.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CBR_Minified.Test
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
    {
        private readonly IServiceScope _scope;

        protected readonly CbrDbContext _dbContext;
        protected readonly EnviromentService _enviromentService;
        protected readonly ICurrencyService _currencyService;
        protected readonly ISaveToStorageService _saveToStorageService;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();

            _dbContext = _scope.ServiceProvider.GetRequiredService<CbrDbContext>();
            _enviromentService = _scope.ServiceProvider.GetRequiredService<EnviromentService>();
            _currencyService = _scope.ServiceProvider.GetRequiredService<ICurrencyService>();
            _saveToStorageService = _scope.ServiceProvider.GetRequiredService<ISaveToStorageService>();
        }

        public virtual void Dispose()
        {
            _scope?.Dispose();
            _dbContext?.Dispose();            
        }
    }
}