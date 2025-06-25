namespace eShop.CatalogService.ConfluentKafka;

public class KafkaProducerOptions
{
    public string BootstrapServers { get; set; }

    public string TopicName { get; set; }
}
