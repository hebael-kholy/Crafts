﻿using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.OrderRepo;

public class OrderRepo : GenericRepo<Order>, IOrderRepo
{
    private readonly CraftsContext _context;
    public OrderRepo(CraftsContext context) : base(context)
    {
        _context = context;
    }

    public Order GetOrderWithCartAndUser(int id)
    {
        return _context.Set<Order>()
            .Include(o => o.User)
            .Include(o => o.Cart)
            .FirstOrDefault(o => o.Id == id)!;
    }

    public List<Order> GetUserOrders(string id)
    {
        return _context.Set<Order>()
                .Where(u => u.UserId == id)
                .Include(o => o.User)
                .Include(o => o.Cart)
                .ToList();
               
    }
}