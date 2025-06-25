// See https://aka.ms/new-console-template for more information

namespace eShop.CatalogService.Console;

using System;
using System.Threading;
using System.Threading.Tasks;
using eShop.CatalogService.BLL.DTOs;
using eShop.CatalogService.MassTransit.Producers;
using global::MassTransit;
using Microsoft.Extensions.DependencyInjection;
using eShop.CatalogService.BLL.DTOs;
using eShop.CatalogService.MassTransit.Models;

public class Program
{
    public static async Task Main()
    {
        var services = new ServiceCollection();

        services.AddScoped<CatalogItemProducer>();
        services.AddMassTransit(x =>
        {
            x.UsingInMemory();

            x.AddRider(rider =>
            {
                rider.AddProducer<ProductUpdateMessage>("catalog-items");

                rider.UsingKafka((context, k) => { k.Host("localhost:29092"); });
            });
        });

        var provider = services.BuildServiceProvider();

        var busControl = provider.GetRequiredService<IBusControl>();

        await busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);

        var producer = provider.GetRequiredService<CatalogItemProducer>();
        try
        {
            //var producer = provider.GetRequiredService<ITopicProducer<KafkaMessage>>();
            do
            {
                string value = await Task.Run(() =>
                {
                    Console.WriteLine("Enter text (or quit to exit)");
                    Console.Write("> ");
                    return Console.ReadLine();
                });

                if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                    break;

                await producer.AddMessage(new ProductInfo { Name = "sdf" });
            } while (true);
        }
        finally
        {
            await busControl.StopAsync();
        }
    }

    public record KafkaMessage
    {
        public string Text { get; init; }
    }
}
