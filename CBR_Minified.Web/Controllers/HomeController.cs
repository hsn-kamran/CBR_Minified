using CBR_Minified.Web.Models;
using CBR_Minified.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CBR_Minified.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISaveToStorageService _saveToStorageService;
        private readonly CbrDbContext _context;

        public HomeController(ISaveToStorageService saveToStorageService, 
            CbrDbContext context,
            EnviromentService enviromentService)
        {
            _saveToStorageService = saveToStorageService;
            _context = context;

            enviromentService.Initialize();
        }

        public async Task<IActionResult> Index()
        {
            await _saveToStorageService.Save();

            var currencies = await _context.CurrencyCourses
                .OrderByDescending(c => c.Date)
                .ThenBy(c => c.CurrencyId)
                .ToListAsync();

            return View(currencies);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
