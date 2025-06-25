// See https://aka.ms/new-console-template for more information
using System.Reflection;
using eShop.CartService;
using eShop.CartService.BLL.DTOs;
using eShop.CartService.BLL.Features.Cart.Commands;
using eShop.CartService.BLL.Interfaces;
using eShop.CartService.BLL.Models;
using eShop.CartService.ConfluentKafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

Log.Logger = Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureAppConfiguration(b =>
    {
        b.AddUserSecrets(Assembly.GetExecutingAssembly());
        b.AddJsonFile("appsettings.json", true);
    })
    .ConfigureLogging((configurationBuilder) =>
    {
        configurationBuilder.AddSerilog().SetMinimumLevel(LogLevel.Debug);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<KafkaConsumerOptions>(context.Configuration.GetSection("Kafka"));
        services.AddSingleton(typeof(IMessageConsumer<>), typeof(KafkaConsumer<>));
        services.AddApplicationLayer();
        services.AddInfrastructureLayer(context.Configuration);
    })
    .Build();


var mediator = host.Services.GetService<IMediator>();

var consumer = host.Services.GetService<IMessageConsumer<ItemUpdatedMessage>>();
consumer.OnMessageReceive += HandleMessage;

CancellationTokenSource source = new CancellationTokenSource();
CancellationToken token = source.Token;

consumer.StartMessageProcessing(token);

var logger = host.Services.GetService<ILogger<Program>>();

logger.LogError("Test log1");

Console.WriteLine("Start processing messages...");
Console.WriteLine("Enter Q to stop");

string input = string.Empty;

while (input != "Q")
{
    input = Console.ReadLine();
}

source.Cancel();
consumer.StopMessageProcessing();

void HandleMessage(ItemUpdatedMessage message)
{
    Console.WriteLine("Updating item...");

    var cartItem = new CartItemInfo
    {
        Id = message.Id,
        Name = message.Name,
        ImageUrl = message.Image,
        Price = message.Price,
        Quantity = message.Quantity
    };

    mediator.Send(new UpdateItemCommand { CartItem = cartItem }).Wait();

    Console.WriteLine("Item updated");
}
