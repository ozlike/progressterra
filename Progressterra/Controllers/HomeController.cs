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


            return View(events);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
