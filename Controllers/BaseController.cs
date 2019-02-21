using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace $safeprojectname$.Controllers
{
    public abstract class BaseController : Controller
    {
        public List<KeyValuePair<string, string>> MetaTags = new List<KeyValuePair<string, string>>();

        public BaseController()
        {
        }

        public override async void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //await Test();
        }

        //private async Task Test()
        //{
        //}
    }
}
