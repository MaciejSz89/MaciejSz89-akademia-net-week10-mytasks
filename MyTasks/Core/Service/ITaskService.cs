using MyTasks.Core.Models;
using MyTasks.Core.Models.Domains;
using MyTasks.Persistence;
using Task = MyTasks.Core.Models.Domains.Task;

namespace MyTasks.Core.Service
{
    public interface ITaskService
    {
        IEnumerable<Task> Get(GetTaskParams param);
        IEnumerable<Category> GetCategories(string userId);
        Task Get(int id, string userId);
        void Add(Task task);
        void Update(Task task);
        void Delete(int id, string userId);
        void Finish(int id, string userId);
    }
}
