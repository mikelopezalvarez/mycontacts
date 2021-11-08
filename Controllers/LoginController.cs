using MyContacts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyContacts.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserVM model)
        {
            try
            {
                using (var db = new mycontactsEntities())
                {
                    var getUser = db.Users.Where(x => x.UserName.Equals(model.UserName)).FirstOrDefault();

                    if (getUser != null)
                    {

                        // Create the authentication ticket with custom user data.
                        var ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now,
                             DateTime.Now.AddMinutes(60),
                             model.Rememberme, FormsAuthentication.FormsCookiePath);
                        //Encrypt the ticket.
                        string encTicket = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        //Create the cookie.                
                        Response.Cookies.Add(cookie);

                        return Json(new
                        {
                            passed = true,
                            url = "/Contact/Index",
                        });

                    }
                    else
                    {
                        return Json(new
                        {
                            passed = false,
                            ex = "The user is not exist"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    passed = false,
                    ex = ex.Message
                });
            }
        }
        [HttpGet]
        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}