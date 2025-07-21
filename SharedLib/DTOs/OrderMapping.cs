using System;
using DataAccess.Models;

namespace SharedLib.DTOs;

public static class OrderMapping
{
    public static Order ToEntity(this OrderDto o) => new()
    {
        ClientName = o.ClientName,
        OrderDate = o.OrderDate
    };

    public static OrderDetailDto ToDetailDto(this Order o) => new()
    {
        OrderId = o.OrderId,
        ClientName = o.ClientName,
        OrderDate = o.OrderDate,
        DetailOrder = o.DetailOrders.Select(o => o.ToDetailDto()).ToList()
    };
}
