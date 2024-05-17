using RabbitMQ.Client;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace FormulaeAPI.Messages
{
    public class MessageProducer : IMessageProducer
    {
        public void SendingMessages<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"
            };

            var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            //create queue
            channel.QueueDeclare("bookings", durable: true, exclusive: false);

            var jsonString = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish("", "bookings", body: body);
        }
    }
}
