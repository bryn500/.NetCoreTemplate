using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using $safeprojectname$.Models;

namespace $safeprojectname$.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData.Add("Title", "SEO Title");
            MetaTags.Add(new KeyValuePair<string, string>("description", "seo description"));
            MetaTags.Add(new KeyValuePair<string, string>("keywords", "seo,keywords,list"));
            ViewBag.MetaNameTags = MetaTags;

            return View();
        }

        [HttpGet("/privacy")]
        public IActionResult Privacy()
        {
            ViewData.Add("Title", "SEO Title");

            return View();
        }
    }
}
