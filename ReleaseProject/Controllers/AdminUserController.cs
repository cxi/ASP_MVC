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
    public class AdminUserController : Controller
    {
        private AccountDBEntities db = new AccountDBEntities();

        //
        // GET: /AdminUser/

        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        //
        // GET: /AdminUser/Details/5

        public ActionResult Details(string id = null)
        {
            //Account account = db.Accounts.Find(id);
            Guid userId = new Guid(id);
            Account account = db.Accounts.FirstOrDefault(u => u.UserId == userId);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        //
        // GET: /AdminUser/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminUser/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                account.UserId = Guid.NewGuid();
                db.Accounts.Add(account);
                if (!ExistEmail(account))
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("error", "Email exist");
            ViewBag.error = TempData["error"];
            return View(account);
        }

        //
        // GET: /AdminUser/Edit/5

        public ActionResult Edit(string id = null)
        {
            //Account account = db.Accounts.Find(id);
            Guid userId = new Guid(id);
            Account account = db.Accounts.FirstOrDefault(u => u.UserId == userId);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        //
        // POST: /AdminUser/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                    if (!ExistEmail(account))
                    {
                        
                        db.Entry(account).State = EntityState.Modified;
                        db.SaveChanges();   
                         return RedirectToAction("Index");
                    }
                }              
            return View(account);
        }

        //
        // GET: /AdminUser/Delete/5

        public ActionResult Delete(string id = null)
        {
            //Account account = db.Accounts.Find(id);
            Guid userId = new Guid(id);
            Account account = db.Accounts.FirstOrDefault(u => u.UserId == userId);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        //
        // POST: /AdminUser/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //Account account = db.Accounts.Find(id);
            Guid userId = new Guid(id);
            Account account = db.Accounts.FirstOrDefault(u => u.UserId == userId);
            if (account != null) { 
                 db.Accounts.Remove(account);
                 db.SaveChanges();
                 return RedirectToAction("Index");
            }
            
            return View(account);
        }


        [HttpGet]
        public ActionResult ShowContact()
        {
            return RedirectToAction("Index", "Contact");
        }

        private bool ExistEmail(Account acc)
        {
            bool existEmail = false;
            using (var db = new AccountDBEntities())
            {
                var user = db.Accounts.FirstOrDefault(u => u.Email == acc.Email);
                if (user != null && user.UserId != acc.UserId)
                {
                    existEmail = true;
                }
            }
            return existEmail;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}