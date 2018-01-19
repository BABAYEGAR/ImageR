using System.Collections.Generic;
using System.Text;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CamerackStudio.Models.RabbitMq
{
    public class SendUserMessage
    {
        public void SendImageCreationMessage(List<AppUser> appUsers)
        {
            //open Rabbit MQ Connection
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                VirtualHost = "/"
            };
            string message = JsonConvert.SerializeObject(appUsers);
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
    }
}
