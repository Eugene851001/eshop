using Asp.Versioning.ApiExplorer;
using eShop.CartService;
using eShop.CartService.API.Extensions;
using eShop.CartService.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication().
    AddJwtBearer(options =>
    {
        builder.Configuration.Bind("Authentication:Jwt", options);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtension(builder.Configuration);

builder.Services.AddApiVersioningExtension();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseMiddleware<TokenLoggingMiddleware>();

app.MapControllers();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});

app.Run();
