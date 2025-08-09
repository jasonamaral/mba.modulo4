using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace BFF.API.Handlers;

public class AuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorization = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(authorization))
        {
            if (request.Headers.Contains("Authorization"))
            {
                request.Headers.Remove("Authorization");
            }

            request.Headers.Add("Authorization", authorization);
        }

        if (!request.Headers.Accept.Any())
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        return base.SendAsync(request, cancellationToken);
    }
}


