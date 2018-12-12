using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tarifX.Models;

namespace tarifX.Controllers
{
    public class AdminKategoriController : Controller
    {
        // GET: Kategori
        tarifXEntities1 ctx = new tarifXEntities1();

        public ActionResult Index()
        {
            return View(ctx.Categories.ToList());
        }

        public ActionResult CategoryFood(int? id)
        {
            if (id==null)
            {
                return HttpNotFound();
            }
            var foods = ctx.Foods.Where(x => x.CategoryID == id).OrderByDescending(x => x.PublishingDate).ToList();
            var cName = ctx.Categories.Single(x=>x.ID==id);
            ViewBag.CategoryName = cName.Name;
            if (foods.Count<1)
            {
                return HttpNotFound();
            }
            return View(foods);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                if (cat.Name!=null)
                {
                    ctx.Categories.Add(cat);
                    ctx.SaveChanges();
                    TempData["Success"] = "Kategori başarıyla eklenmiştir.";    
                }
                else
                {
                    TempData["Fail"]= "Kategori eklenmesi sırasında bir hata oluştu. Lütfen bilgileri kontrol edip tekrar deneyiniz.";
                }
                return RedirectToAction("Create");

            }
            return View(cat);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var cat = ctx.Categories.FirstOrDefault(x => x.ID == id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            return View(cat);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id) //JAVASCRIPT ALERT İLE SİL.
        {
            var cat = ctx.Categories.FirstOrDefault(x => x.ID == id);
            ctx.Categories.Remove(cat);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}