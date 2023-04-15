using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Core.Models;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Service;
using MyTasks.Core.ViewModels;
using MyTasks.Persistence.Exceptions;
using MyTasks.Persistence.Extensions;
using MyTasks.Persistence.Services;

namespace MyTasks.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Categories()
        {
            var userId = User.GetUserId();

            var categories = _categoryService.Get(userId);

            return View(categories);
        }


        public IActionResult CategoriesTable()
        {
            var userId = User.GetUserId();

            var categories = _categoryService.Get(userId);

            return PartialView("_CategoriesTable", categories);
        }

        [HttpPost]
        public IActionResult Category(int id = 0)
        {
            var userId = User.GetUserId();

            var vm = new CategoryViewModel
            {
                Heading = id == 0 ? "Dodawanie nowej kategorii" : "Edycja kategorii",
                Category = id == 0 ? new Category { UserId = userId } : _categoryService.Get(id, userId)
            };

            return PartialView("_Category", vm);
        }

        [HttpPost]
        public IActionResult Save(Category category)
        {
            try
            {
                var userId = User.GetUserId();
                category.UserId = userId;
                if (category.Id != 0)
                {
                    _categoryService.Update(category);
                }

                else
                {
                    _categoryService.Add(category);
                }

            }
            catch (Exception ex)
            {
                //logowanie
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true });
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _categoryService.Delete(id, userId);
            }
            catch(ReferencedToAnotherObjectException)
            {
                return Json(new { success = false, message = "Kategoria jest przypisana do zadania i nie może być usunięta" });
            }
            catch (Exception ex)
            {
                //logowanie
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true });
        }
    }
}
