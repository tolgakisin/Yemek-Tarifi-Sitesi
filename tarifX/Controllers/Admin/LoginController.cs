using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tarifX.Models;

namespace tarifX.Controllers.Admin {
    public class LoginController : Controller {
        // GET: Login
        tarifXEntities1 ctx = new tarifXEntities1();

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Login login) {
            var userDetails = ctx.Logins.Where(x => x.UserName == login.UserName && x.Password == login.Password).FirstOrDefault();
            if (userDetails!=null) {
                Session["userID"] = userDetails.ID;
                return RedirectToAction("Index","AdminYemek");
            }
            return View();
        }

        public ActionResult LogOff() {
            Session.Clear();
            return RedirectToAction("Index","Login");
        }
    }
}