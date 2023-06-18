using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;

namespace Crafts.DAL.Repos.OrderRepo;

public interface IOrderRepo : IGenericRepo<Order>
{
    Order GetOrderWithCartAndUser(int id);
    List<Order> GetUserOrders(string id);
}
