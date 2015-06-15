using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.ServiceReference1;

namespace WebApp.Controllers
{
    public class PanelController : Controller
    {
        //
        // GET: /Panel/
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            Service1Client service = new Service1Client();

            List<Customer> customerList = new List<Customer>();

            customerList = service.GetAllCutomers();

            return View(customerList);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateCustomerStatus(List<Customer> model)
        {
            if (model != null)
            {
                Service1Client service = new Service1Client();
                Dictionary<int, int> customerNewValues = new Dictionary<int, int>();

                foreach (Customer customer in model)
                {
                    if (customer.Status != null)
                    {
                        customerNewValues.Add(customer.Id, (int)customer.Status);
                    }
                }

                service.UpdateCustomerStatus(customerNewValues);
            }

            return RedirectToAction("Index", "Ambassador");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ViewCustomerDetail(int Id)
        {
            Service1Client service = new Service1Client();

            var customer = service.GetCustomerById(Id);
            var address = service.GetAddressByOwnerId(Id);

            CustomerCompleteModel customerComplete = new CustomerCompleteModel();

            customerComplete.Name = customer.Name;
            customerComplete.LastName = customer.LastName;
            customerComplete.Email = customer.Email;
            customerComplete.Mobile = customer.Mobile;
            customerComplete.Phone = customer.Phone;
            customerComplete.Address = address.Street;
            customerComplete.ExtNum = address.ExtNum;
            customerComplete.IntNum = address.IntNum;
            customerComplete.ResidentialArea = address.ResidencialArea;
            customerComplete.City = address.City;
            customerComplete.State = address.State;
            customerComplete.ZipCode = address.ZipCode;

            return View("CustomerDetail", customerComplete);
        }
    }
}
