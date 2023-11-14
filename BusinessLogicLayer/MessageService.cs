using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using DataAccessLayer.Entities;
using RabbitMQ.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BusinessLogicLayer
{

    public class MessageService : IMessageService
    {
        private readonly string _hostName = "localhost";
        private readonly string _queueName = "item";

        public string Publish(Item item)
        {
            var factory = new ConnectionFactory { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queueName,
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

            var message = JsonSerializer.Serialize(item);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                routingKey: _queueName,
                                basicProperties: null,
                                body: body);

            return $" [x] Sent {message}";
        }
    }
}
