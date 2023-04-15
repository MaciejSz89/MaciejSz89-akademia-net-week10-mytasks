using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Core;
using MyTasks.Core.Models;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Service;
using MyTasks.Core.ViewModels;
using MyTasks.Persistence;
using MyTasks.Persistence.Extensions;
using MyTasks.Persistence.Services;
using Task = MyTasks.Core.Models.Domains.Task;

namespace MyTasks.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private ITaskService _taskService;
        private ICategoryService _categoryService;

        public TaskController(ITaskService taskService, ICategoryService categoryService)
        {
            _taskService = taskService;
            _categoryService = categoryService;
        }
        public IActionResult Tasks()
        {
            var userId = User.GetUserId();

            var vm = new TasksViewModel
            {
                Tasks = _taskService.Get(new GetTaskParams { UserId = userId }),
                Categories = _categoryService.Get(userId),
                FilterTasks = new FilterTasks()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Tasks(TasksViewModel viewModel)
        {
            var userId = User.GetUserId();

            var tasks = _taskService.Get(new GetTaskParams
            {
                UserId = userId,
                IsExecuted = viewModel.FilterTasks.IsExecuted,
                CategoryId = viewModel.FilterTasks.CategoryId,
                Title = viewModel.FilterTasks.Title
            });


            return PartialView("_TasksTable", tasks);

        }


        public IActionResult Task(int id = 0)
        {
            var userId = User.GetUserId();

            var task = id == 0 ? new Task { Id = 0, UserId = userId, Term = DateTime.Today } : _taskService.Get(id, userId);

            var vm = new TaskViewModel
            {
                Task = task,
                Categories = _categoryService.Get(userId),
                Heading = id == 0 ? "Dodawanie nowego zadania" : "Edytowanie zadania"
            };

            return View(vm);

        }

        [HttpPost]
        public IActionResult Task(Task task)
        {
            var userId = User.GetUserId();
            task.UserId = userId;


            if (!ModelState.IsValid)
            {
                var vm = new TaskViewModel
                {
                    Task = task,
                    Categories = _categoryService.Get(userId),
                    Heading = task.Id == 0 ? "Dodawanie nowego zadania" : "Edytowanie zadania"
                };

                return View("Task", vm);
            }


            if (task.Id == 0)
            {
                _taskService.Add(task);
            }
            else
            {
                _taskService.Update(task);
            }

            return RedirectToAction("Tasks");

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _taskService.Delete(id, userId);
            }
            catch (Exception ex)
            {
                //logowanie
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Finish(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _taskService.Finish(id, userId);
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
