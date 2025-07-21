using System;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens.Experimental;
using SharedLib.DTOs;

namespace BusinessLogic.Endpoints;

public static class DetailOrderEndpoints
{
    public static IEndpointRouteBuilder MapDetailOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Orders/Details");

        group.MapPost("/Create", CreateDetailOrder);
        group.MapGet("{orderDetailIdFk}", GetDetailOrder);

        return routes;
    }

    private static async Task<IResult> CreateDetailOrder(
        [FromBody] DetailOrderDto detailOrderDto,
        [FromServices] DetailOrderService detailOrderService
    )
    {
        var errors = detailOrderDto.ValidateDto();

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        var detailOrder = await detailOrderService.SaveDetailOrder(detailOrderDto);

        if (detailOrder != null)
        {
            return Results.Ok(detailOrder.ToDetailDto());
        }

        errors.Add("Product", ["Error inesperado"]);
        return Results.BadRequest(errors);
    }

    private static async Task<IResult> GetDetailOrder(
        [FromRoute] int orderDetailIdFk,
        [FromServices] DetailOrderService detailOrderService
    )
    {
        var errors = new Dictionary<string, string[]>();

        var detailOrder = await detailOrderService.GetDetailOrder(orderDetailIdFk);
        var detailOrderDtos = detailOrder?.Select(o => o.ToDetailDto()).ToList(); //Devuelve un NULL

        if (detailOrderDtos is not null)
        {
            return Results.Ok(detailOrderDtos);
        }

        errors.Add("Product", ["Error inesperado"]);
        return Results.BadRequest(errors);
    }
}
