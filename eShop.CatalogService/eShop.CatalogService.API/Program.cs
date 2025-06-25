using eShop.CatalogService.API.Constants;
using eShop.CatalogService.API.Extensions;
using eShop.CatalogService.API.Middleware;
using eShop.CatalogService.BLL;
using eShop.CatalogService.BLL.Interfaces;
using eShop.CatalogService.BLL.Services;
using eShop.CatalogService.ConfluentKafka;
using eShop.CatalogService.DAL;
using eShop.CatalogService.DAL.Contexts;
using Microsoft.AspNetCore.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog();

Log.Logger = Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.ConfigureLogging((configurationBuilder) =>
{
    configurationBuilder.AddSerilog().SetMinimumLevel(LogLevel.Debug);
}); 

builder.Services.AddApiVersioningExtension();

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();

builder.Services.Configure<KafkaProducerOptions>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddScoped(typeof(IMessageProducer<>), typeof(KafkaProducer<>));

builder.Services.AddControllers();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSwaggerExtension(builder.Configuration);

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        builder.Configuration.Bind("Authentication:Jwt", options);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthPoliciesConstants.Edit, pb =>
    {
        pb.RequireAuthenticatedUser();
        pb.RequireRole(RoleConstants.Manager);
    });

    options.AddPolicy(AuthPoliciesConstants.Read, pb =>
    {
        pb.RequireAuthenticatedUser();
        pb.RequireRole(RoleConstants.Manager, RoleConstants.Customer);
    });

    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

var logger = app.Services.GetService<Microsoft.Extensions.Logging.ILogger<Program>>();

logger.LogError("Test log");

app.Logger.LogInformation("Seeding Database...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var connectionString = app.Configuration.GetConnectionString("DefaultConnection");
        app.Logger.LogInformation($"Connection string: {connectionString}");

        var catalogContext = scopedProvider.GetRequiredService<ApplicationDbContext>();
        var categiriesService = scopedProvider.GetRequiredService<CategoryService>();

        await ApplicationDbContextSeed.SeedAsync(catalogContext, categiriesService);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Logger.LogInformation("Database initialized");

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();
