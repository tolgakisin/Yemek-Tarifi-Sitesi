using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tarifX.Models;

namespace tarifX.Controllers.Admin {
    public class HomeController : Controller {
        // GET: Home
        tarifXEntities1 ctx = new tarifXEntities1();

        public ActionResult Index() {
            return View(ctx.Foods.ToList().OrderByDescending(x => x.PublishingDate).Take(6));
        }

        public ActionResult About() {
            ViewBag.RecipeCount = ctx.Foods.Count();
            ViewBag.CategoryCount = ctx.Categories.Count();
            ViewBag.SentByPeopleFoodCount = ctx.Foods.Where(x => x.PublishedBy != "Admin").Count();
            return View();
        }

        public ActionResult Category(int? id) {
            if (id==null) {
                return HttpNotFound();
            }
            var foods = ctx.Foods.Where(x => x.CategoryID == id).OrderByDescending(x => x.PublishingDate).ToList();
            if (foods == null) {
                return HttpNotFound();
            }
            var categoryName = ctx.Categories.Single(x => x.ID == id);
            ViewBag.CategoryName = categoryName.Name;
            return View(foods);
        }
    }
}