using System;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLib.DTOs;

namespace BusinessLogic.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Orders");

        group.MapPost("/Create", CreateOrder);
        group.MapGet("", GetOrders);

        return routes;
    }

    private static async Task<IResult> CreateOrder(
        [FromBody] OrderDto oderDto,
        [FromServices] OrderService orderService
    )
    {
        var errors = oderDto.ValidateDto();

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        var order = await orderService.SaveOrder(oderDto);

        if (order != null)
        {
            return Results.Ok(order.ToDetailDto());
        }

        errors.Add("Order", ["Error inesperado"]);
        return Results.BadRequest(errors);
    }

    private static async Task<IResult> GetOrders(
        [FromServices] OrderService orderService
    )
    {
        var errors = new Dictionary<string, string[]>();

        var orders = await orderService.GetOrders();
        var orderDtos = orders?.Select(o => o.ToDetailDto()).ToList();

        if (orderDtos is not null)
        {
            return Results.Ok(orderDtos);
        }

        errors.Add("Product", ["Error inesperado"]);
        return Results.BadRequest(errors);
    }
}
