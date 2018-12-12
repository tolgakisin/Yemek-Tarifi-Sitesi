using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tarifX.Models;

namespace tarifX.Controllers.Admin {
    public class AdminGonderilenTariflerController : Controller {
        // GET: AdminGonderilenTarifler
        tarifXEntities1 ctx = new tarifXEntities1();

        public ActionResult Index() {
            return View(ctx.FoodFromPeoples.ToList());
        }

        public ActionResult Details(int? id) {
            if (id == null) {
                return HttpNotFound();
            }
            var food = ctx.FoodFromPeoples.FirstOrDefault(x => x.ID == id);

            if (food == null) {
                return HttpNotFound();
            }
            return View(food);
        }

        [ActionName("Details")]
        [HttpPost]
        public ActionResult AddToFood(int? id) {
            if (id == null) {
                return HttpNotFound();
            }
            var food = ctx.FoodFromPeoples.FirstOrDefault(x => x.ID == id);

            if (food == null) {
                return HttpNotFound();
            }

            Food f = new Food {
                Title = food.Title,
                PublishingDate = food.PublishingDate,
                PublishedBy = food.PublishedBy,
                PreparationTime = food.PreparationTime,
                Photo = food.Photo,
                NumberOfPeople = food.NumberOfPeople,
                Materials = food.Materials,
                CookingTime = food.CookingTime,
                Content = food.Content,
                CategoryID = food.CategoryID
            };

            System.IO.File.Move(@"C:\Users\tolga\source\repos\tarifX\tarifX\Uploads\RecipePhotoFromPeople\" + food.Photo, @"C:\Users\tolga\source\repos\tarifX\tarifX\Uploads\RecipePhoto\" + f.Photo);

            ctx.Foods.Add(f);
            ctx.FoodFromPeoples.Remove(food);
            ctx.SaveChanges();

            return RedirectToAction("Index"); ;

        }


        public ActionResult Delete(int? id) {
            if (id == null) {
                return HttpNotFound();
            }
            var food = ctx.FoodFromPeoples.FirstOrDefault(x => x.ID == id);

            if (food == null) {
                return HttpNotFound();
            }
            return View(food);
        }

        [ActionName("Delete")]
        [HttpPost]
        public ActionResult DeleteConfirmed(int? id) {
            var food = ctx.FoodFromPeoples.FirstOrDefault(x => x.ID == id);

            if (food.Photo != null) {
                if (System.IO.File.Exists(Server.MapPath(food.Photo))) {
                    System.IO.File.Delete(Server.MapPath(food.Photo));
                }
            }

            ctx.FoodFromPeoples.Remove(food);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}