using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace UI.Handlers;

//HttpCLient Handler that adds the JWT automaticly 
public class JwtAuthorizationMessageHandler(ProtectedLocalStorage localStorage) : DelegatingHandler
{
    private readonly ProtectedLocalStorage _localStorage = localStorage;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var result = await _localStorage.GetAsync<string>("authToken");
        var token = result.Success ? result.Value : null;

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
