using System;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;

namespace BusinessLogic.Services;

public class OrderService(BjxDbContext context, ILogger<OrderService> logger)
{
    private readonly BjxDbContext _context = context;
    private readonly ILogger _logger = logger;

    public async Task<Order?> SaveOrder(OrderDto orderDto)
    {
        try
        {
            var order = orderDto.ToEntity();
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR]: {ex.Message} - {ex.StackTrace}");
            return null;
        }
    }

    public async Task<IList<Order>?> GetOrders()
    {
        try
        {
            return await _context.Orders.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR]: {ex.Message} - {ex.StackTrace}");
            return null;
        }
    }
}
