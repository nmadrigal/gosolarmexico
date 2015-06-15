using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ServiceReference1;

namespace WebApp.Controllers
{
    public class MapController : Controller
    {
        //
        // GET: /Map/
        [Authorize]
        public ActionResult Index()
        {
            Service1Client service = new Service1Client();

            ViewBag.AddressList = service.GetAllAddresses();

            return View();
        }

    }
}
