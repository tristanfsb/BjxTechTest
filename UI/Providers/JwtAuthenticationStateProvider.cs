using System;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace UI.Providers;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;

    public JwtAuthenticationStateProvider(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var result = await _localStorage.GetAsync<string>("authToken");
        var token = result.Success ? result.Value : null;

        var identity = string.IsNullOrWhiteSpace(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    // Puedes usar un método para parsear claims desde el JWT
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
{
    var claims = new List<Claim>();
    var payload = jwt.Split('.')[1]; // Obtener el payload (segunda parte del token)
    
    // Añadir relleno base64 si es necesario
    var jsonBytes = ParseBase64WithoutPadding(payload);
    
    // Deserializar el JSON a un diccionario de clave-valor
    var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
    
    if (keyValuePairs == null) 
        return claims;

    // Extraer los claims
    claims.AddRange(keyValuePairs.Select(kvp => 
    {
        // Manejar arrays (ej: roles)
        if (kvp.Value is JsonElement element && element.ValueKind == JsonValueKind.Array)
        {
            var arrayClaims = new List<Claim>();
            foreach (var arrayItem in element.EnumerateArray())
            {
                arrayClaims.Add(new Claim(kvp.Key, arrayItem.ToString()));
            }
            return arrayClaims;
        }
        return new List<Claim> { new Claim(kvp.Key, kvp.Value.ToString()) };
    }).SelectMany(x => x));

    return claims;
}

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        // Añadir relleno faltante
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        // Convertir de Base64Url a Base64 estándar
        return Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/'));
    }

}