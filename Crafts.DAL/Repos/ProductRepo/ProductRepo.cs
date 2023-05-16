using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Crafts.DAL.Repos.ProductsRepo
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        private readonly CraftsContext _context;

        public ProductRepo(CraftsContext context):base(context) 
        {
            _context = context;
        }

    }
}
