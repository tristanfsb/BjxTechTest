using System;
using DataAccess.Models;

namespace SharedLib.DTOs;

public static class ProductMapping
{
    public static Product ToEntity(this ProductDto p) => new()
    {
        ProductName = p.ProductName!,
        Description = p.Description ?? "",
        Stock = p.Stock!.Value,
        Code = p.Code!
    };

    public static ProductDetailDto ToDetailDto(this Product p) => new()
    {
        ProductId = p.ProductId,
        ProductName = p.ProductName,
        Description = p.Description ?? "",
        Stock = p.Stock,
        Code = p.Code!

    };
}
