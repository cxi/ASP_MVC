using ReleaseProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace ReleaseProject.Controllers
{   
    public class AccountController : Controller
    {

        private Account userAccount = new Account();
        //
        // GET: /User/

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.AccountModel user)
        {
            int isAdmin;
            if (ModelState.IsValid)
            {
                if (IsValid(user.UserEmail, user.Password, out isAdmin))
                {
                    FormsAuthentication.SetAuthCookie(user.UserEmail, false);
                    Response.SetCookie(new HttpCookie("UserId", userAccount.UserId.ToString()));
                    //if log in as administrator user
                    if (isAdmin == 1)
                    {
                        return RedirectToAction("Index", "AdminUser");
                    }
                    //if login as normal user
                    else
                    {
                        return RedirectToAction("Index", "Contact");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email/Password incorrect");
                }
            }
            return View();
        }


        //for reset password
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ForgotPassword(Models.RestPassModel userEmail)
        {
            using (var db = new AccountDBEntities())
            {
                if (userEmail != null)
                {
                    var user = db.Accounts.FirstOrDefault(u => u.Email == userEmail.UserEmail);
                    if (user == null)
                    {
                        ModelState.AddModelError("error", "Email does not exist");
                    }
                    else
                    {
                        ModelState.AddModelError("error", "New password send to you ,please Check");
                    }
                }
                ModelState.AddModelError("error", "Email does not exist");
            }
            
            return View(userEmail);
        }
        
        //To validate 
        private bool IsValid(string email, string password, out int isadmin)
        {
            var crypto = new SimpleCrypto.PBKDF2();

            isadmin = 0;
            bool isValid = false;
            using (var db = new AccountDBEntities())
            {
                var user = db.Accounts.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    if (user.Password == password)
                    {
                        isValid = true;
                        isadmin = user.UserType;
                        userAccount = user;
                    }
                }
            }
            return isValid;
        }
    }
}
