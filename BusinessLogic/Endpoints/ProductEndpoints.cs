using System;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLib.DTOs;

namespace BusinessLogic.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Products");

        group.MapPost("/Create", CreateProduct);
        group.MapGet("", GetProducts);

        return routes;
    }

    private static async Task<IResult> CreateProduct(
        [FromBody] ProductDto productDto,
        [FromServices] ProductService productService
    )
    {
        var errors = productDto.ValidateDto();

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        var product = await productService.SaveProduct(productDto);

        if (product != null)
        {
            return Results.Ok(product.ToDetailDto());
        }

        errors.Add("Product", ["Error inesperado"]);
        return Results.BadRequest(errors);
    }

    private static async Task<IResult> GetProducts(
        [FromServices] ProductService productService
    )
    {
        var errors = new Dictionary<string, string[]>();

        var products = await productService.GetProducts();
        var productDtos = products?.Select(p => p.ToDetailDto()).ToList();

        if (productDtos is not null)
        {
            return Results.Ok(productDtos);
        }

        errors.Add("Product", ["Error inesperado"]);
        return Results.BadRequest(errors);
    }
}
