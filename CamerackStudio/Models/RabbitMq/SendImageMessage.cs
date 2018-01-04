using System.Text;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CamerackStudio.Models.RabbitMq
{
    public class SendImageMessage
    {
        public void SendImageCreationMessage(ImageUpload upload)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost", RequestedHeartbeat = 5
            };
            string message = JsonConvert.SerializeObject(upload);
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "CamerackTaskScheduler",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                    routingKey: "CamerackTaskScheduler",
                    basicProperties: properties,
                    body: body);
            }
        }
        public void SendImageActionMessage(ImageAction action)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                RequestedHeartbeat = 5
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "CamerackTaskScheduler",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(action);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                    routingKey: "CamerackTaskScheduler",
                    basicProperties: properties,
                    body: body);
            }
        }
    }
}
