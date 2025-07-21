using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharedLib.DTOs;
using System.Text.Json;

namespace WebUI.Providers;

public class WasmAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage, ILogger<WasmAuthenticationStateProvider> logger) : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILocalStorageService _localStorage = localStorage;
    private readonly ILogger<WasmAuthenticationStateProvider> _logger = logger;


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Obtener el objeto usuario del localStorage
            var user = await _localStorage.GetItemAsync<UserDetailDto>("auth_user");

            // Si no hay usuario o token, retornar estado vacío
            if (user == null || string.IsNullOrWhiteSpace(user.Token))
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsIdentity identity = new ClaimsIdentity();

            // Verificar si el token es válido y puede ser leído
            if (tokenHandler.CanReadToken(user.Token))
            {
                try
                {
                    // Leer y validar el token
                    var jwtToken = tokenHandler.ReadJwtToken(user.Token);
                    var claims = jwtToken.Claims.ToList();

                    if (!claims.Any(c => c.Type == ClaimTypes.Role))
                    {
                        // fallback: intenta obtener desde "role" o "roles" si no está ya mapeado
                        if (jwtToken.Payload.TryGetValue("role", out var roleObj))
                        {
                            if (roleObj is string roleStr)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, roleStr));
                            }
                            else if (roleObj is JsonElement element && element.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var item in element.EnumerateArray())
                                {
                                    claims.Add(new Claim(ClaimTypes.Role, item.GetString()));
                                }
                            }
                        }
                    }

                    // Verificar expiración
                    var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
                    if (expClaim != null && long.TryParse(expClaim.Value, out long expUnix))
                    {
                        var expTime = DateTimeOffset.FromUnixTimeSeconds(expUnix);
                        if (expTime < DateTimeOffset.UtcNow)
                        {
                            await _localStorage.RemoveItemAsync("auth_user");
                            return new AuthenticationState(new ClaimsPrincipal());
                        }
                    }

                    identity = new ClaimsIdentity(claims, "jwt");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", user.Token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al leer el token JWT");
                    await _localStorage.RemoveItemAsync("auth_user");
                }
            }

            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationState(principal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en GetAuthenticationStateAsync");
            return new AuthenticationState(new ClaimsPrincipal());
        }
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
