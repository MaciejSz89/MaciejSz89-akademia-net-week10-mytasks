using MyTasks.Core.Models.Domains;

namespace MyTasks.Core.Service
{
    public interface ICategoryService
    {
        IEnumerable<Category> Get(string userId);
        Category Get(int id, string userId);
        void Update(Category category);
        void Add(Category category);
        void Delete(int id, string userId);
    }
}
