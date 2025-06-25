using Asp.Versioning;

using Microsoft.OpenApi.Models;

namespace eShop.CatalogService.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            // Specify the default API Version as 1.0
            config.DefaultApiVersion = new ApiVersion(1, 0);
            // If the client hasn't specified the API version in the request, use the default API version number 
            config.AssumeDefaultVersionWhenUnspecified = true;
            // Advertise the API versions supported for the particular endpoint
            config.ReportApiVersions = true;
        });

        return services;
    }

    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(o =>
        {
            o.AddSecurityDefinition("Keycloak", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(configuration["Keycloak:AuthorizationUrl"]),
                        Scopes = new Dictionary<string, string>
                        {
                            ["openid"] = "openid",
                            ["profile"] = "profile"
                        }
                    }
                }
            });

            var securityRequirement = new OpenApiSecurityRequirement();

            securityRequirement.Add(new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Keycloak",
                    Type = ReferenceType.SecurityScheme,
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer"
            }, []);

            o.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }
}
