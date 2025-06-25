using System.Text.Json;

using Confluent.Kafka;
using Confluent.Kafka.Admin;
using eShop.CatalogService.BLL.Interfaces;

using Microsoft.Extensions.Options;

namespace eShop.CatalogService.ConfluentKafka;

public class KafkaProducer<T>(IOptions<KafkaProducerOptions> options) : IMessageProducer<T> where T : class
{
    public async Task Produce(T message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            var serializedMessage = JsonSerializer.Serialize(message);

            await producer.ProduceAsync(options.Value.TopicName, new Message<Null, string> { Value = serializedMessage });
        }
    }

    public async Task CreateTopic(string topicName)
    {
        using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = options.Value.BootstrapServers }).Build())
        {
            try
            {
                await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification { Name = options.Value.TopicName, ReplicationFactor = 1, NumPartitions = 1 } });
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }
    }
}
