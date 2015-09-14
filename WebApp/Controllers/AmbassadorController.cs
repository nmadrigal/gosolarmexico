using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ServiceReference1;

namespace WebApp.Controllers
{
    public class AmbassadorController : Controller
    {
        //
        // GET: /Ambassador/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
