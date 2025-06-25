using System.Text.Json;
using Confluent.Kafka;
using eShop.CartService.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eShop.CartService.ConfluentKafka;

public class KafkaConsumer<T> : IDisposable, IMessageConsumer<T> where T : class
{
    private readonly ConsumerConfig _config;
    private readonly string _topic;
    private readonly ILogger _logger;

    private IConsumer<Ignore, string> _consumer;

    public event Action<T> OnMessageReceive;

    public KafkaConsumer(IOptions<KafkaConsumerOptions> options, ILogger<KafkaConsumer<T>> logger)
    {
        _config = new ConsumerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = options.Value.TopicName,
            EnableAutoCommit = false,
        };

        _topic = options.Value.TopicName;

        _logger = logger;
    }

    public void StopMessageProcessing()
    {
        _consumer.Close();
        _consumer.Dispose();
    }

    public async Task StartMessageProcessing(CancellationToken cancellationToken = default)
    {
        var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        consumer.Subscribe([_topic]);

        _logger.LogInformation($"Subscribed to topic {_topic}");

        await Task.Run(() =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cancellationToken);

                try
                {
                    _logger.LogInformation("Start processing message");
                    var serializationOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var message = JsonSerializer.Deserialize<T>(consumeResult.Message.Value, serializationOptions);

                    OnMessageReceive?.Invoke(message);

                    consumer.Commit();
                }
                catch (JsonException ex)
                {
                    _logger.LogError("Invalid message in queue {0}", consumeResult.Message.Value);
                    consumer.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while processing message: {0}", ex.Message);
                }
            }
        });
    }

    public void Dispose()
    {
        StopMessageProcessing();
    }
}
