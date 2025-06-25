using System.Reflection;
using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace eShop.CartService.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiVersioningExtension(this IServiceCollection services)
    {
        var versionBuilder = services.AddApiVersioning(config =>
        {
            // Specify the default API Version as 1.0
            config.DefaultApiVersion = new ApiVersion(1, 0);
            // If the client hasn't specified the API version in the request, use the default API version number 
            config.AssumeDefaultVersionWhenUnspecified = true;
            // Advertise the API versions supported for the particular endpoint
            config.ReportApiVersions = true;
        });

        versionBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Cart Service Api - V1",
                    Version = "v1",
                    Description = "API for managing eShop cart items",
                });

            setup.SwaggerDoc(
                "v2",
                new OpenApiInfo
                {
                    Title = "Cart Service Api - V2",
                    Version = "v2",
                    Description = "API for managing eShop cart items",
                });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //setup.EnableAnnotations();
            setup.IncludeXmlComments(xmlPath);

            setup.AddSecurityDefinition("Keycloak", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
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

            setup.AddSecurityRequirement(securityRequirement);

        });

        return services;
    }
}
