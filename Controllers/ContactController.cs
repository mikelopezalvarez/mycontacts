using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyContacts.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult GetAll(string search)
        {
            using (var db = new MyContacts_dbEntities())
            {

                var contacts = from g in db.Contacts where g.Name.Contains(search) || g.eMail.Contains(search) || g.Phone.Contains(search) || g.Fax.Contains(search) select g;

                return Json(new { data = contacts.ToList() }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult GetDetail(int id)
        {
            using (var db = new MyContacts_dbEntities())
            {
                Contacts contacts = db.Contacts.Find(id);

                var obj = JsonConvert.SerializeObject(contacts);

                JsonResult res = Json(new
                {
                    data = obj
                });

                return res;

            }
        }

    
        

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name,Phone,Fax,eMail,Notes,LastUpdateDate")] Contacts contacts)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MyContacts_dbEntities())
                {
                    db.Contacts.Add(contacts);
                    db.SaveChanges();
                    return Json(new { data = true });
                }
            }

            return Json(new { data = false });
        }
 

        [HttpPost]
        public ActionResult Edit([Bind(Include = "contactID,Name,Phone,Fax,eMail,Notes,LastUpdateDate")] Contacts contacts)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MyContacts_dbEntities())
                {
                    db.Entry(contacts).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { data = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { data = false });

        }

       

    }
}