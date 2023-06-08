using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace DiscountAPI.Services;

public class MessageProducer : IMessageProducer
{
  private readonly IConfiguration _config;

  public MessageProducer(IConfiguration config)
  {
    _config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, false)
    .Build();
  }
  public void SendingMessage<T>(T message)
  {
    var factory = new ConnectionFactory()
    {
      HostName = _config.GetSection("RabbitMQConnectionStrings")["Hostname"],
      UserName = _config.GetSection("RabbitMQConnectionStrings")["Username"],
      Password = _config.GetSection("RabbitMQConnectionStrings")["Password"],
      Port = int.Parse(_config.GetSection("RabbitMQConnectionStrings")["Port"]),
      VirtualHost = _config.GetSection("RabbitMQConnectionStrings")["VirtualHost"],
    };

    using (var conn = factory.CreateConnection())
    using (var channel = conn.CreateModel())
    {
      channel.ExchangeDeclare(exchange: "CAPSTONE_EXCHANGE", type: ExchangeType.Direct, durable: true);
      channel.QueueDeclare(queue: "DISCOUNT", durable: true, exclusive: false);
      channel.QueueBind(queue: "DISCOUNT", exchange: "CAPSTONE_EXCHANGE", routingKey: "DISCOUNT_SERVICE");

      var jsonString = JsonSerializer.Serialize(message);

      var body = Encoding.UTF8.GetBytes(jsonString);

      channel.BasicPublish(exchange: "CAPSTONE_EXCHANGE", routingKey: "DISCOUNT_SERVICE", body: body);

      Console.WriteLine("SendingMessage Done");
    }
  }
}
