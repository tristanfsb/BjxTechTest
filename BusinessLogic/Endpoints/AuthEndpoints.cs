
using SharedLib.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogic.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Auth");

        group.MapPost("/Login", LoginUser);

        return routes;
    }

    private static async Task<IResult> LoginUser(
        [FromBody] LoginDTO loginDto,
        [FromServices] AuthService authService,
        [FromServices] ILogger<LoginDTO> logger
    )
    {
        var errors = loginDto.ValidateDto();

        if (errors.Count > 0)
        {
            return Results.BadRequest(errors);
        }

        logger.LogWarning("PREPARANDO LOGIN");

        var user = await authService.Login(loginDto);

        if (user != null)
        {
            var token = authService.GenerateToken(user.UserId, user.Email, user.Role);

            return Results.Ok(user.ToDetailDto(token));
        }

        errors.Add("User", ["Credenciales incorrectas"]);
        return Results.BadRequest(errors);
    }

}
