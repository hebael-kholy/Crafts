using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using Microsoft.EntityFrameworkCore;

namespace Crafts.DAL.Repos.CategoryRepo
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        private readonly CraftsContext _context;
        public CategoryRepo(CraftsContext context) : base(context)
        {
            _context = context;
        }
        public Category? GetByIdWithProducts(int id)
        {
            //Explicit Loading
            var x = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);
            return x;
        }

        public Category? GetCategoryByTitle(string title)
        {
            var cat = _context.Categories.Include(c=>c.Products).FirstOrDefault(c=>c.Title==title);
            return cat;
        }
    }
}
