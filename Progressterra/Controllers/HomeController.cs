using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Progressterra.Context;
using Progressterra.Models;
using Progressterra.Services;

namespace Progressterra.Controllers
{
    public class HomeController : Controller
    {
        IDataProvider dataProvider;
        public HomeController(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public async Task<IActionResult> Index()
        {
            var events = await dataProvider.InterrogateServices();

            ViewBag.MaxResponseTime = dataProvider.GetMaxResponseTime();


            return View(events);
        }

        public async Task<IActionResult> Service(int? id)
        {
            if (!id.HasValue) return RedirectToAction("Index", "Home");
            
            return View(dataProvider.GetEventsForService((int)id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
