using System;
using DataAccess.Models;

namespace SharedLib.DTOs;

public static class DetailOrderMapping
{

    public static DetailOrder ToEntity(this DetailOrderDto d) => new()
    {
        OrderIdFk = d.OrderIdFk!.Value,
        ProductIdFk = d.ProductIdFk!.Value,
        Quantity = d.Quantity!.Value
    };

    public static DetailOrderDetailDto ToDetailDto(this DetailOrder d) => new()
    {
        DetailId = d.DetailId,
        Code = d.ProductIdFkNavigation.Code!,
        OrderIdFk = d.OrderIdFk,
        ProductIdFk = d.ProductIdFk,
        Quantity = d.Quantity
    };
}
