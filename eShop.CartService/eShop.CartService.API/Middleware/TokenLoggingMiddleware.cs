using System.IdentityModel.Tokens.Jwt;

namespace eShop.CartService.API.Middleware;

public class TokenLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenLoggingMiddleware> _logger;

    public TokenLoggingMiddleware(RequestDelegate next, ILogger<TokenLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the Authorization header contains a Bearer token
        if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader) &&
            authorizationHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authorizationHeader.ToString().Substring("Bearer ".Length).Trim();

            try
            {
                // Decode the token and log its claims
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    _logger.LogInformation("ID Token Details:");

                    foreach (var claim in jwtToken.Claims)
                    {
                        _logger.LogInformation("Claim Type: {Type}, Claim Value: {Value}", claim.Type, claim.Value);
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid token format.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while decoding the ID token.");
            }
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}
