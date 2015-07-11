﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Account()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Ambassador");
            }
            else
            {
                return View("Login", "_MobileLayout");
                //return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return PartialView("~/Partials/_Contact");
        }
    

        [HttpPost]
        public string ContactForm(ContactModel contact)
        {
            //if (ModelState.IsValid)
            //{
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add("nestor_madrigal@live.com");
                    mail.From = new MailAddress(contact.Email);
                    mail.Subject = "Mensaje de parte de: " + contact.Name + " del estado de: " + contact.State.Text;
                    mail.Body = contact.Message;
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient("relay-hosting.secureserver.net");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("admin@gosolarmexico.com.mx", "G@Solar2015mail");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = false;

                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            //}

            return "OK";
        }


    }
}
