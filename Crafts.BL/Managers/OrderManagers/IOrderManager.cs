using Crafts.BL.Dtos.OrderDtos;
using Crafts.BL.Dtos.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.OrderManagers
{
    public interface IOrderManager
    {
        List<OrderReadDto> GetAll();
        List<OrderReadDto> GetUserOrders(string id);
        Task Add(OrderAddDto order);
        OrderReadDto GetById(int id);
        void Edit(OrderEditDto orderEditDto, int id);
        void CancelOrder(int id);
        void AcceptOrder(int id);
        void Delete(int id);
    }
}
