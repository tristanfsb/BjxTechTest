using System;
using SharedLib.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogic.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Users");

        group.MapPost("/Create", CreateUser);
        group.MapGet("", GetUsers);

        return routes;
    }

    private static async Task<IResult> CreateUser(
        [FromBody] UserDTO userDto,
        [FromServices] UserService userService
    )
    {
        var errors = userDto.ValidateDto();

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        var user = await userService.SaveUser(userDto);

        if (user != null)
        {
            return Results.Ok(user.ToDetailDto());
        }

        errors.Add("User", ["Error inesperado"]);
        return Results.BadRequest(errors);
    }

    private static async Task<IResult> GetUsers(
        [FromServices] UserService userService
    )
    {
        var errors = new Dictionary<string, string[]>();

        var users = await userService.GetUsers();
        var userDtos = users?.Select(u => u.ToDetailDto()).ToList();

        if (users is not null)
        {
            return Results.Ok(userDtos);
        }

        errors.Add("Users", ["Error inesperado"]);
        return Results.BadRequest(errors);
    }
}
