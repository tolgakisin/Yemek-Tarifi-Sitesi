using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tarifX.Models;
using Newtonsoft.Json;

namespace tarifX.Controllers.Home
{
    public class ContactController : Controller
    {
        // GET: Contact
        tarifXEntities1 ctx = new tarifXEntities1();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact contact) {

            if (ModelState.IsValid) {
                try {
                    contact.CreatedDate = System.DateTime.Now;
                    ctx.Contacts.Add(contact);

                    var response = Request["g-recaptcha-response"];
                    const string secret = "6LdpVnUUAAAAADrdMKJeTxoWa_GKEoFh3JXInGuN";

                    var client = new WebClient();
                    var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

                    var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                    if (!captchaResponse.Success)
                        TempData["Message"] = "Please verify the re-captcha.";
                    else
                        TempData["Success"] = "Email has been sent successfully.";
                } catch (Exception) {
                    TempData["Message"] = "Please verify the re-captcha.";
                    return View();
                }


                ctx.SaveChanges();
                return RedirectToAction("Index","Contact");
            }

            // maybe we can add one more page that is about the message has been sent successfully.
            return View(contact);
        }
    }
}