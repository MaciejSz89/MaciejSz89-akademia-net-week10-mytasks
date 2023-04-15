using Microsoft.EntityFrameworkCore;
using MyTasks.Core;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Repositories;
using MyTasks.Persistence.Exceptions;
using System.Threading.Tasks;

namespace MyTasks.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private IApplicationDbContext _context;
        public CategoryRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Category> Get(string userId)
        {
            var categories = _context.Categories
                                     .Where(x => x.UserId == userId);

            return _context.Categories
                           .Where(x => x.UserId == userId)
                           .OrderBy(x => x.Name)
                           .ToList();
        }

        public Category Get(int id, string userId)
        {
            return _context.Categories
                           .Single(x => x.UserId == userId
                                     && x.Id == id);
        }
        public void Update(Category category)
        {
            var categoryToUpdate = _context.Categories
                                           .Single(x => x.UserId == category.UserId
                                                     && x.Id == category.Id);
            categoryToUpdate.Name = category.Name;

            _context.Categories.Update(categoryToUpdate);
        }

        public void Delete(int id, string userId)
        {
            var categoryToDelete = _context.Categories
                                           .Include(x => x.Tasks)
                                           .Single(x => x.UserId == userId
                                                     && x.Id == id);

            if (categoryToDelete.Tasks != null && categoryToDelete.Tasks.Any())
                throw new ReferencedToAnotherObjectException();

            _context.Categories.Remove(categoryToDelete);
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }
    }
}
