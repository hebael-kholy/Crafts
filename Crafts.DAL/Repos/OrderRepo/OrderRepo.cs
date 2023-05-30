using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.OrderRepo;

public class OrderRepo : GenericRepo<Order>, IOrderRepo
{
    public OrderRepo(CraftsContext context) : base(context)
    {
    }
}
