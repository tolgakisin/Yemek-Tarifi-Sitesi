using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tarifX.Models;

namespace tarifX.Controllers.Admin
{
    public class AdminIletisimController : Controller
    {
        // GET: AdminIletisim
        tarifXEntities1 ctx = new tarifXEntities1();

        public ActionResult Mailbox() {
            var mails = ctx.Contacts.ToList();

            return View(mails);
        }

        public ActionResult BatchDelete(int[] deleteInputs) {
            if (deleteInputs != null && deleteInputs.Length > 0) {
                foreach (var item in deleteInputs) {
                    var mail = ctx.Contacts.FirstOrDefault(x => x.ID == item);
                    ctx.Contacts.Remove(mail);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Mailbox");
        }

        public ActionResult ReadMail(int? id) {
            if (id == null) {
                return HttpNotFound();
            }
            var mail = ctx.Contacts.SingleOrDefault(x => x.ID == id);

            return View(mail);
        }

    }
}