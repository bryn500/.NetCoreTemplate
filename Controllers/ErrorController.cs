using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using $safeprojectname$.Models;

namespace $safeprojectname$.Controllers
{
    [Route("error")]
    public class ErrorController : BaseController
    {
        public ErrorController() : base()
        {
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("500")]
        public IActionResult ServerError()
        {
            return View();
        }
    }
}
