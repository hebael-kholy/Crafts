using Crafts.BL.Dtos.CartItemsDtos;
using Crafts.BL.Dtos.OrderDtos;
using Crafts.BL.Dtos.ReviewDtos;
using Crafts.DAL.Models;
using Crafts.DAL.Models.Enum;
using Crafts.DAL.Repos.CartItemsRepo;
using Crafts.DAL.Repos.CartRepo;
using Crafts.DAL.Repos.IdentityRepo;
using Crafts.DAL.Repos.OrderRepo;
using Crafts.DAL.Repos.ProductsRepo;
using Crafts.DAL.Repos.ReviewRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.OrderManagers;

 public class OrderManager : IOrderManager
{
    private readonly IOrderRepo _orderRepo;
    private readonly ICartRepo _cartRepo;
    private readonly ICartItemRepo _cartItemRepo;
    private readonly IUserRepo _userRepo;

    public OrderManager(IOrderRepo orderRepo, ICartRepo cartRepo, IUserRepo userRepo , ICartItemRepo cartItemRepo)
    {
        _orderRepo = orderRepo;
        _cartRepo = cartRepo;
        _userRepo = userRepo;
        _cartItemRepo = cartItemRepo;
    }
 
    public List<OrderReadDto> GetAll()
    {
        List<Order> ordersFromDb = _orderRepo.GetOrderWithCartItems();

        List<OrderReadDto> orderReadDtos = new List<OrderReadDto>();

        foreach(var o in  ordersFromDb)
        {
            var user = _userRepo.GetUserById(o.UserId);

            var orderReadDto = new OrderReadDto
            {
                Id = o.Id,
                Status = o.Status,
                PaymentMethod = o.PaymentMethod,
                TotalPrice = o.TotalPrice,
                TaxPrice = o.TaxPrice,
                ShippingPrice = o.ShippingPrice,
                PaidAt = o.PaidAt,
                CreatedAt = o.CreatedAt,
                IsPaid = o.IsPaid,
                CartId = o.CartId,
                UserId = o.UserId,
                UserName = user!.UserName!,
                cartItems = o.Cart.CartItems.Select(ci => new CartItemsChildReadDto
                {
                    Id = ci.Id,
                    Title = ci.Product.Title,
                    Description = ci.Product.Description,
                    Image = ci.Product.Image,
                    Quantity = ci.Quantity,
                 
                    Rating = ci.Product.Rating,
                    CategoryId = ci.Product.CategoryId
                }).ToList()
            };
            
            orderReadDtos.Add(orderReadDto);
        }
        return orderReadDtos;
    }

    public List<OrderReadDto> GetByStatus(string id, int status)
    {
        List<Order> ordersFromDb = _orderRepo.GetUserOrdersByStatus(id, status);
        return ordersFromDb.Select(o => new OrderReadDto
        {
            Id = o.Id,
            Status = o.Status,
            PaymentMethod = o.PaymentMethod,
            TotalPrice = o.TotalPrice,
            TaxPrice = o.TaxPrice,
            ShippingPrice = o.ShippingPrice,
            PaidAt = o.PaidAt,
            CreatedAt = o.CreatedAt,
            IsPaid = o.IsPaid,
            CartId = o.CartId,
            UserId = o.UserId,
            
            cartItems = o.Cart.CartItems.Select(ci => new CartItemsChildReadDto
            {
                Id = ci.Id,
                Title = ci.Product.Title,
                Description = ci.Product.Description,
                Image = ci.Product.Image,
                Quantity=ci.Product.Quantity,
                Rating = ci.Product.Rating,
                CategoryId = ci.Product.CategoryId,
                Price = ci.Product.Price
             
            }).ToList()

        }).ToList();
    }

    public OrderReadDto GetById(int id)
    {
        var order = _orderRepo.GetOrderWithCartAndUser(id);
        if (order != null)
        {
            return new OrderReadDto
            {
                Id = id,
                Status = order.Status,
                PaymentMethod = order.PaymentMethod,
                TotalPrice = order.TotalPrice,
                TaxPrice = order.TaxPrice,
                ShippingPrice = order.ShippingPrice,
                PaidAt = order.PaidAt,
                CreatedAt = order.CreatedAt,
                IsPaid = order.IsPaid,
                CartId = order.CartId,
                UserId = order.UserId
              
            };
        }
        else
        {
            throw new ArgumentException($"Order with id {id} is not found");
        }
    }

    public async Task Add(OrderAddDto orderAddDto)
    {
        // Check if the cart, user with the given ID exist
        var cart = _cartRepo.GetById(orderAddDto.CartId);
        var cartPrice = 0.0;
        var totalOrderPrice = 0.0;

        
        var shippingPrice = 20;
        
        
        if (cart == null)
        {
            throw new ArgumentException($"Cart with id {orderAddDto.CartId} is not found");
        }

        var user = _userRepo.GetUserById(orderAddDto.UserId.ToString());
        if (user == null)
        {
            throw new ArgumentException($"User with id {orderAddDto.UserId} is not found");
        }
        if (cart.TotalPriceAfterDiscount != null)
        {
            cartPrice = cart.TotalPriceAfterDiscount; 
            

        }
        else
        {
            cartPrice = cart.TotalPrice;
            
        }

        var taxPrice = 0.1 * cartPrice;
        totalOrderPrice = cartPrice + shippingPrice + taxPrice;


        Order orderToAdd = new Order
        {
            Status = Status.Pending,
            PaymentMethod = PaymentMethod.Cash,
            TotalPrice = totalOrderPrice,
            TaxPrice = taxPrice,
            ShippingPrice = shippingPrice,
            PaidAt = DateTime.Now,
            CreatedAt = DateTime.Now,
            IsPaid = false,
            CartId = orderAddDto.CartId,
            UserId = orderAddDto.UserId
        };
        await _orderRepo.Add(orderToAdd);

        _orderRepo.SaveChanges();
        //_cartItemRepo.DeleteAllCartItemsByCartId(cart.Id);
        //_cartItemRepo.SaveChanges();

    }

    public void Edit(OrderEditDto orderEditDto, int id)
    {
        // Check if the cart, user with the given ID exist
        var cart = _cartRepo.GetById(orderEditDto.CartId);
        if (cart == null)
        {
            throw new ArgumentException($"Cart with id {orderEditDto.CartId} is not found");
        }

        var user = _userRepo.GetUserById(orderEditDto.UserId.ToString());
        if (user == null)
        {
            throw new ArgumentException($"User with id {orderEditDto.UserId} is not found");
        }

        var orderToEdit = _orderRepo.GetById(id);
        if (orderToEdit != null)
        {
            orderToEdit.Status = orderEditDto.Status;
            orderToEdit.PaymentMethod = orderEditDto.PaymentMethod;
            orderToEdit.TotalPrice = orderEditDto.TotalPrice;
            orderToEdit.TaxPrice = orderEditDto.TaxPrice;
            orderToEdit.ShippingPrice = orderEditDto.ShippingPrice;
            orderToEdit.PaidAt = orderEditDto.PaidAt;
            orderToEdit.CreatedAt = orderEditDto.CreatedAt;
            orderToEdit.IsPaid = orderEditDto.IsPaid;
            orderToEdit.UserId = orderEditDto.UserId;
            orderToEdit.CartId = orderEditDto.CartId;

            _orderRepo.Update(orderToEdit);
            _orderRepo.SaveChanges();
        }
        else
        {
            throw new ArgumentException($"Order with id {id} is not found");
        }
    }
    public void Delete(int id)
    {
        var orderToDelete = _orderRepo.GetById(id);

        if (orderToDelete != null)
        {
            _orderRepo.Delete(orderToDelete);
            _orderRepo.SaveChanges();
        }
        else
        {
            throw new ArgumentException($"Order with id {id} is not found");
        }
    }

    public List<OrderReadDto> GetUserOrders(string id)
    {
        var user = _userRepo.GetUserById(id.ToString());
        if (user == null)
        {
            throw new ArgumentException($"User with id {id} is not found");
        }
        List<Order> ordersFromDb = _orderRepo.GetUserOrders(id);
        return ordersFromDb.Select(o => new OrderReadDto
        {
            Id = o.Id,
            Status = o.Status,
            PaymentMethod = o.PaymentMethod,
            TotalPrice = o.TotalPrice,
            TaxPrice = o.TaxPrice,
            ShippingPrice = o.ShippingPrice,
            PaidAt = o.PaidAt,
            CreatedAt = o.CreatedAt,
            IsPaid = o.IsPaid,
            CartId = o.CartId,
            UserId = o.UserId
        }).ToList();

    }

    public void CancelOrder(int id)
    {

        var orderToEdit = _orderRepo.GetById(id);
        if (orderToEdit != null)
        {
            orderToEdit.Status = Status.Rejected;

            _orderRepo.Update(orderToEdit);
            _orderRepo.SaveChanges();
        }
        else
        {
            throw new ArgumentException($"Order with id {id} is not found");
        }
    }

    

    public void AcceptOrder(int id)
    {

        var orderToEdit = _orderRepo.GetById(id);
        if (orderToEdit != null)
        {
            orderToEdit.Status = Status.Accepted;

            _orderRepo.Update(orderToEdit);
            _orderRepo.SaveChanges();
        }
        else
        {
            throw new ArgumentException($"Order with id {id} is not found");
        }
    }
}