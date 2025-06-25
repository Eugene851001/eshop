namespace eShop.CartService.ConfluentKafka;

public class KafkaConsumerOptions
{
    public string BootstrapServers { get; set; }

    public string GroupId { get; set; }

    public string TopicName { get; set; }
}
