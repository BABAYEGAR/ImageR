using System.Text;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CamerackStudio.Models.RabbitMq
{
    public class SendEmailMessage
    {
        public void SendCompetitionEmailMessage(Competition competition)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
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

                string message = JsonConvert.SerializeObject(competition);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "CamerackTaskScheduler",
                    basicProperties: properties,
                    body: body);
            }
        }
    }
}
