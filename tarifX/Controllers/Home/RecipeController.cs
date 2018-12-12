using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using tarifX.Models;
using PagedList.Mvc;
using PagedList;

namespace tarifX.Controllers.Admin {
    public class RecipeController : Controller {
        // GET: Recipe
        tarifXEntities1 ctx = new tarifXEntities1();

        public ActionResult Index(string searchString, int? page) {
            var foods = ctx.Foods.OrderByDescending(x => x.PublishingDate).ToList();

            if (searchString != null) {
                page = 1;
            }

            if (!String.IsNullOrEmpty(searchString)) {
                foods = ctx.Foods.Where(x => x.Title.ToUpper().Contains(searchString.ToUpper())
                || x.Category.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View(foods.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Details(int? id) {
            if (id == null) {
                return HttpNotFound();
            }
            var food = ctx.Foods.SingleOrDefault(x => x.ID == id);
            if (food == null) {
                return HttpNotFound();
            }
            return View(food);
        }

        public ActionResult SendFood() {
            ViewBag.CategoryList = new SelectList(GetCategoryList(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult SendFood(FoodFromPeople food, HttpPostedFileBase Photo) {
            if (ModelState.IsValid) {
                if (Photo != null) {
                    WebImage img = new WebImage(Photo.InputStream);
                    FileInfo photoInfo = new FileInfo(Photo.FileName);

                    string newPhoto = Guid.NewGuid().ToString() + photoInfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/RecipePhotoFromPeople/" + newPhoto); //,"png",true
                    food.Photo = newPhoto;
                }


                //food.PublishedBy = "Admin";
                food.PublishingDate = System.DateTime.Now;

                ctx.FoodFromPeoples.Add(food);
                ctx.SaveChanges();
                if (ctx.Foods.Where(x => x.ID == food.ID) != null) {
                    TempData["Success"] = "Yemek tarifi başarıyla gönderilmiştir.";
                } else {
                    TempData["Fail"] = "Yemek tarifi gönderilmesi sırasında bir hata oluştu. Lütfen bilgileri kontrol edip tekrar deneyiniz.";
                }
                return RedirectToAction("SendFood");
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

    }
}