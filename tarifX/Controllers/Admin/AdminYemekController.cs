using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using tarifX.Models;

namespace tarifX.Controllers {
    public class AdminYemekController : Controller {
        // GET: Admin
        tarifXEntities1 ctx = new tarifXEntities1();

        public ActionResult Index() {

            return View(ctx.Foods.ToList().OrderByDescending(x => x.PublishingDate));
        }

        public ActionResult Details(int? id) {
            if (id == null) {
                return HttpNotFound();
            }
            var food = ctx.Foods.FirstOrDefault(x => x.ID == id);

            if (food == null) {
                return HttpNotFound();
            }
            return View(food);
        }

        private List<SelectListItem> GetCategoryList() {
            return ctx.Categories
                .Select(c => new SelectListItem {
                    Value = c.ID.ToString(),
                    Text = c.Name
                }).ToList();

        }

        public ActionResult Create() {

            ViewBag.CategoryList = new SelectList(GetCategoryList(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Food food, HttpPostedFileBase Photo) {
            if (ModelState.IsValid) {
                if (Photo != null) {
                    WebImage img = new WebImage(Photo.InputStream);
                    FileInfo photoInfo = new FileInfo(Photo.FileName);

                    string newPhoto = Guid.NewGuid().ToString() + photoInfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/RecipePhoto/" + newPhoto); //,"png",true
                    food.Photo = newPhoto;
                }


                food.PublishedBy = "Admin";
                food.PublishingDate = System.DateTime.Now;

                ctx.Foods.Add(food);
                ctx.SaveChanges();
                if (ctx.Foods.Where(x => x.ID == food.ID) != null) {
                    TempData["Success"] = "Yemek tarifi başarıyla oluşturulmuştur.";
                } else {
                    TempData["Fail"] = "Yemek tarifi oluşturulması sırasında bir hata oluştu. Lütfen bilgileri kontrol edip tekrar deneyiniz.";
                }
                return RedirectToAction("Create");
            }

            return View(food);
        }

        public ActionResult Edit(int? id) {
            if (id == null) {
                return HttpNotFound();
            }
            ViewBag.CategoryList = new SelectList(GetCategoryList(), "Value", "Text");
            var food = ctx.Foods.FirstOrDefault(x => x.ID == id);
            if (food == null) {
                return HttpNotFound();
            }
            return View(food);
        }

        [HttpPost]
        public ActionResult Edit(int? id, Food food, HttpPostedFileBase Photo) {
            if (id == null) {
                return HttpNotFound();
            }

            if (ModelState.IsValid) {
                var editedFood = ctx.Foods.FirstOrDefault(x => x.ID == id);

                if (Photo != null) {
                    if (System.IO.File.Exists(Server.MapPath(food.Photo))) {
                        System.IO.File.Delete(Server.MapPath(food.Photo));
                    }

                    WebImage img = new WebImage(Photo.InputStream);
                    FileInfo photoInfo = new FileInfo(Photo.FileName);
                    string newPhoto = Guid.NewGuid().ToString() + photoInfo.Extension;

                    img.Resize(800, 350);
                    img.Save("~/Uploads/RecipePhoto/" + newPhoto);
                    food.Photo = newPhoto;
                    editedFood.Photo = food.Photo;
                }

                editedFood.Title = food.Title;
                editedFood.CategoryID = food.CategoryID;
                editedFood.Content = food.Content;
                editedFood.Materials = food.Materials;
                editedFood.NumberOfPeople = food.NumberOfPeople;
                editedFood.PreparationTime = food.PreparationTime;
                editedFood.CookingTime = food.CookingTime;

                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(food);
        }

        public ActionResult Delete(int? id) {
            if (id == null) {
                return HttpNotFound();
            }
            var food = ctx.Foods.FirstOrDefault(x => x.ID == id);
            if (food == null) {
                return HttpNotFound();
            }
            return View(food);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id) {
            var food = ctx.Foods.FirstOrDefault(x => x.ID == id);

            if (food.Photo != null) {
                if (System.IO.File.Exists(Server.MapPath(food.Photo))) {
                    System.IO.File.Delete(Server.MapPath(food.Photo));
                }
            }

            ctx.Foods.Remove(food);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}