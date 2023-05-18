using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.CategoryRepo
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        private readonly CraftsContext _context;
        public CategoryRepo(CraftsContext context):base(context) 
        {
            _context = context;
        }
        public Category GetCategoryWithProducts(int id)
        {
            return _context.Set<Category>()
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id)!;
        }
    }
}
