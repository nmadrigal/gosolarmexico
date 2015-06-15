using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using WebMatrix.WebData;
using WebApp.Filters;
using WebApp.Models;
using WebApp.ServiceReference1;
using System.Net.Mail;
using System.Net;
using System.Xml;
using System.Web.Script.Serialization;


namespace WebApp.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        Service1Client service;

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                service = new Service1Client();

                if (service.UserAuthentication(User.Identity.Name, model.OldPassword))
                {
                    service.ChangePasswordByUserEmail(User.Identity.Name, model.NewPassword);

                    return RedirectToAction("Index", "Ambassador");
                }
                else
                {
                    ModelState.AddModelError("", "El password actual no es correcto");
                }
            }
            return View(model);
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)// && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                service = new Service1Client();

                if (service.UserAuthentication(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    return RedirectToAction("Index", "Ambassador");
                }

                //Proxy proxy = new Proxy();

                //if (proxy.UserAuthentication(model.UserName, model.Password))
                //{

                //}                
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.Session.RemoveAll();

            FormsAuthentication.SignOut();
            //WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CustomerModel model)
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
                    customer.IsAmbassador = true;
                    customer.ReferencedByEmail = "";

                    Address address = new Address();

                    address.Type = 0;
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

                    MailMessage mail = new MailMessage();

                    mail.To.Add(customer.Email);
                    mail.Bcc.Add("jferrelmty@gmail.com");
                    mail.Bcc.Add("jhonamp@gmail.com");
                    mail.From = new MailAddress("admin@gosolarmexico.com.mx");
                    mail.Subject = "Bienvenido Embajador Solar!";
                    mail.Body = "<p>Hola " + customer.Name + " " + customer.LastName + "</p><br/><p>Nos complace darte la bienvenida como embajador de Go Solar Mexico! <br/> Tu password es: <strong>"
                        + password + "</strong></p><br/><p>Entra a www.gosolarmexico.com.mx y cambia tu password!<p/><br/><p><strong>Go Solar Mexico</strong><p/>";
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient("relay-hosting.secureserver.net");
                    //SmtpClient smtp = new SmtpClient("smtpout.secureserver.net", 25);
                    //SmtpClient smtp = new SmtpClient("smtpout.secureserver.net", 80);
                    //smtp.UseDefaultCredentials = false;
                    //smtp.Credentials = new System.Net.NetworkCredential("admin@gosolarmexico.com.mx", "G@Solar2015mail");
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.EnableSsl = false;

                    smtp.Send(mail);

                    return RedirectToAction("Index", "Team");
                }

                ModelState.AddModelError("", "E-mail ya existe, introduzca otro.");
            }
            return View("Register", model);
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

        private void GoogleGeoCode(ref Address address)
        {
            //string url = "http://maps.googleapis.com/maps/api/geocode/json?sensor=true&address=";

            //dynamic googleResults = new Uri(url + address).GetDynamicJsonObject();
            //foreach (var result in googleResults.results)
            //{
            //    Console.WriteLine("[" + result.geometry.location.lat + "," + result.geometry.location.lng + "] " + result.formatted_address);
            //}
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Disassociate(string provider, string providerUserId)
        {
            //string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            //ManageMessageId? message = null;

            //// Only disassociate the account if the currently logged in user is the owner
            //if (ownerAccount == User.Identity.Name)
            //{
            //    // Use a transaction to prevent the user from deleting their last login credential
            //    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            //    {
            //        bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //        if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
            //        {
            //            OAuthWebSecurity.DeleteAccount(provider, providerUserId);
            //            scope.Complete();
            //            message = ManageMessageId.RemoveLoginSuccess;
            //        }
            //    }
            //}

            //return RedirectToAction("Manage", new { Message = message });
        }


        //
        // GET: /Account/Manage
        [Authorize]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            //ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Manage(LocalPasswordModel model)
        {
            //bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //ViewBag.HasLocalPassword = hasLocalAccount;
            //ViewBag.ReturnUrl = Url.Action("Manage");
            //if (hasLocalAccount)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        // ChangePassword will throw an exception rather than return false in certain failure scenarios.
            //        bool changePasswordSucceeded;
            //        try
            //        {
            //            changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
            //        }
            //        catch (Exception)
            //        {
            //            changePasswordSucceeded = false;
            //        }

            //        if (changePasswordSucceeded)
            //        {
            //            return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
            //        }
            //        else
            //        {
            //            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            //        }
            //    }
            //}
            //else
            //{
            //// User does not have a local password so remove any validation errors caused by a missing
            //// OldPassword field
            //ModelState state = ModelState["OldPassword"];
            //if (state != null)
            //{
            //    state.Errors.Clear();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
            //        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
            //    }
            //    catch (Exception)
            //    {
            //        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
            //    }
            //}
            //}

            // If we got this far, something failed, redisplay form
            //return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public void ExternalLoginCallback(string returnUrl)
        {
            //AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            //if (!result.IsSuccessful)
            //{
            //    return RedirectToAction("ExternalLoginFailure");
            //}

            //if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            //{
            //    return RedirectToLocal(returnUrl);
            //}

            //if (User.Identity.IsAuthenticated)
            //{
            //    // If the current user is logged in add the new account
            //    OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
            //    return RedirectToLocal(returnUrl);
            //}
            //else
            //{
            //    // User is new, ask for their desired membership name
            //    string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
            //    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
            //    ViewBag.ReturnUrl = returnUrl;
            //    return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            //}
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public void ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            //string provider = null;
            //string providerUserId = null;

            //if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            //{
            //    return RedirectToAction("Manage");
            //}

            //if (ModelState.IsValid)
            //{
            //    // Insert a new user into the database
            //    using (UsersContext db = new UsersContext())
            //    {
            //        UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
            //        // Check if user already exists
            //        if (user == null)
            //        {
            //            // Insert name into the profile table
            //            db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
            //            db.SaveChanges();

            //            OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
            //            OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

            //            return RedirectToLocal(returnUrl);
            //        }
            //        else
            //        {
            //            ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
            //        }
            //    }
            //}

            //ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            //ViewBag.ReturnUrl = returnUrl;
            //return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public void ExternalLoginsList(string returnUrl)
        {
            //ViewBag.ReturnUrl = returnUrl;
            //return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public void RemoveExternalLogins()
        {
            //ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            //List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            //foreach (OAuthAccount account in accounts)
            //{
            //    AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

            //    externalLogins.Add(new ExternalLogin
            //    {
            //        Provider = account.Provider,
            //        ProviderDisplayName = clientData.DisplayName,
            //        ProviderUserId = account.ProviderUserId,
            //    });
            //}

            //ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                //OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
