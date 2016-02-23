using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReleaseProject.Domain;

namespace ReleaseProject.Controllers
{
    public class ContactController : Controller
    {
        private ContactEntities db = new ContactEntities();
        Guid userId = new Guid();
        //
        // GET: /Contact/

        public ActionResult Index()
        {
            if (CheckCookies())
            {
                var hasData = db.Contacts.FirstOrDefault(u => u.UserId == userId);
                if (hasData != null)
                {
                    return View(db.Contacts.Where(u => u.UserId == userId).Select(u=>u).ToList());
                }
            }
            return View();
        }

        //
        // GET: /Contact/Details/5

        public ActionResult Details(Guid id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        //
        // GET: /Contact/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Contact/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact contact)
        {
            CheckCookies();
            if (ModelState.IsValid)
            {
                contact.ContactId = Guid.NewGuid();
                contact.UserId = userId; 
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        //
        // GET: /Contact/Edit/5

        public ActionResult Edit(Guid id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        //
        // POST: /Contact/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        //
        // GET: /Contact/Delete/5

        public ActionResult Delete(Guid id )
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        //
        // POST: /Contact/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool CheckCookies()
        {
            bool hasCookies = false;
            if (Request.Cookies["UserId"] != null)
            {
                userId = new Guid(Request.Cookies["UserId"].Value);
                hasCookies = true;
            }
            return hasCookies;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}