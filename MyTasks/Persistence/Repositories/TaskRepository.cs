using Microsoft.EntityFrameworkCore;
using MyTasks.Core;
using MyTasks.Core.Models;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Repositories;
using Task = MyTasks.Core.Models.Domains.Task;

namespace MyTasks.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private IApplicationDbContext _context;

        public TaskRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Task> Get(GetTaskParams param)
        {
            var tasks = _context.Tasks
                                .Include(x => x.Category)
                                .Where(x => x.UserId == param.UserId
                                    && x.IsExecuted == param.IsExecuted);

            if (param.CategoryId != 0)
                tasks = tasks.Where(x => x.CategoryId == param.CategoryId);


            if (!string.IsNullOrWhiteSpace(param.Title))
                tasks = tasks.Where(x => x.Title.Contains(param.Title));

            return tasks.OrderBy(x => x.Term).ToList();
        }


        public Task Get(int id, string userId)
        {
            var task = _context.Tasks.Single(x => x.Id == id
                                               && x.UserId == userId);

            return task;
        }

        public void Add(Task task)
        {
            _context.Tasks.Add(task);
        }

        public void Update(Task task)
        {
            var taskToUpdate = _context.Tasks
                                       .Single(x => x.UserId == task.UserId
                                                 && x.Id == task.Id);

            taskToUpdate.Title = task.Title;
            taskToUpdate.Description = task.Description;
            taskToUpdate.CategoryId = task.CategoryId;
            taskToUpdate.IsExecuted = task.IsExecuted;
            taskToUpdate.Term = task.Term;

            _context.Tasks.Update(taskToUpdate);

        }

        public void Delete(int id, string userId)
        {
            var taskToDelete = _context.Tasks
                                       .Single(x => x.UserId == userId
                                                 && x.Id == id);

            _context.Tasks.Remove(taskToDelete);
        }

        public void Finish(int id, string userId)
        {
            var taskToUpdate = _context.Tasks
                                       .Single(x => x.UserId == userId
                                                 && x.Id == id);

            taskToUpdate.IsExecuted = true;

            _context.Tasks.Update(taskToUpdate);
        }
    }
}
