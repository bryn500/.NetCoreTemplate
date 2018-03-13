using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using $safeprojectname$.Models;

namespace $safeprojectname$.Controllers
{
    [ResponseCache(CacheProfileName = "Default")]
    //[Route("")] // prefix
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            const string testSessionName = "testSession";
            var serialisedDate = JsonConvert.SerializeObject(DateTime.Now);
            HttpContext.Session.SetString(testSessionName, serialisedDate);

            ViewData.Add("Title", "Homepage");

            return View();
        }

        [HttpGet("problem")]
        public IActionResult Problem()
        {
            return StatusCode(500);
        }
    }
}
