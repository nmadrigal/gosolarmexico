using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebApp.Models;
using WebApp.ServiceReference1;
using WebMatrix.WebData;


namespace WebApp.Controllers
{
    public class TeamController : Controller
    {
        Service1Client service;

        //
        // GET: /Team/
        [Authorize]
        public ActionResult Index()
        {
            service = new Service1Client();

            List<ServiceReference1.Customer> customerList = service.GetReferralsByCustomerEmail(User.Identity.Name);

            ViewBag.customerList = customerList;

            return View();
        }

        [Authorize]
        public ActionResult NewReferral()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddReferral(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                service = new Service1Client();

                if (!service.CustomerMailExists(Request["email"]))
                {
                    string password = Membership.GeneratePassword(6, 2);
                    Customer customer = new Customer();
                    customer.Name = Request["Name"];
                    customer.LastName = Request["LastName"];
                    customer.Email = Request["Email"];
                    customer.Phone = Request["Phone"];
                    customer.Mobile = Request["Mobile"];
                    customer.Password = password;
                    customer.ReferencedById = 0;
                    customer.IsAmbassador = false;
                    customer.ReferencedByEmail = HttpContext.User.Identity.Name;
                    customer.Status = 0;

                    Address address = new Address();

                    address.Type = 1;
                    address.Street = Request["Street"];
                    address.ExtNum = Request["ExternalNum"];
                    if (!String.IsNullOrEmpty(Request["InteriorNum"]))
                    {
                        address.IntNum = Request["InteriorNum"];
                    }
                    address.ResidencialArea = Request["ResidentialArea"];
                    address.City = Request["City"];
                    address.ZipCode = Convert.ToInt32(Request["ZipCode"]);
                    address.State = Request["State"];
                    address.Country = "Mexico";

                    var addressComplete = address.Street + " " + address.ExtNum + ", " + address.ResidencialArea + ", " + address.City + ", " + address.State + ", " + address.Country + ", " + address.ZipCode;

                    using (var client = new WebClient())
                    {
                        string searchUrl = string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false", addressComplete);
                        string geocodeInfo = client.DownloadString(searchUrl);

                        JavaScriptSerializer jss = new JavaScriptSerializer();

                        GoogleGeoCodeResponse response = jss.Deserialize<GoogleGeoCodeResponse>(geocodeInfo);

                        address.Latitude = Convert.ToDecimal(response.results[0].geometry.location.lat);
                        address.Longitude = Convert.ToDecimal(response.results[0].geometry.location.lng);
                    }      

                    service.NewReferral(customer, address);

                    //MailMessage mail = new MailMessage();
                    //mail.To.Add(customer.Email);
                    //mail.From = new MailAddress("admin@gosolarmexico.com.mx");
                    //mail.Subject = "Saludos de Go Solar Mexico!";
                    //mail.Body = "<p>Hola " + customer.Name + " " + customer.LastName + "</p><br/><p>" + HttpContext.User.Identity.Name
                    //    + " te ah recomendado con nosotros para ofrecerte una solucion de ahorro de energia.<br/> Tu password es: <strong>"
                    //    + password + "</strong></p><br/><p>Entra a www.gosolarmexico.com.mx y cambia tu password!<p/><br/><p><strong>Go Solar Mexico</strong><p/>";
                    //mail.IsBodyHtml = true;

                    //SmtpClient smtp = new SmtpClient("relay-hosting.secureserver.net");
                    //smtp.UseDefaultCredentials = false;
                    //smtp.Credentials = new System.Net.NetworkCredential("admin@gosolarmexico.com.mx", "G@Solar2015mail");
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.EnableSsl = false;

                    //smtp.Send(mail);

                    return RedirectToAction("Index", "Team");
                }

                ModelState.AddModelError("", "E-mail ya existe, introduzca otro.");
            }
            return View("NewReferral", model);
        }

        public class GoogleGeoCodeResponse
        {

            public string status { get; set; }
            public results[] results { get; set; }

        }

        public class results
        {
            public string formatted_address { get; set; }
            public geometry geometry { get; set; }
            public string[] types { get; set; }
            public address_component[] address_components { get; set; }
        }

        public class geometry
        {
            public string location_type { get; set; }
            public location location { get; set; }
        }

        public class location
        {
            public string lat { get; set; }
            public string lng { get; set; }
        }

        public class address_component
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public string[] types { get; set; }
        }
    }
}
