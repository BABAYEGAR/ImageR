using System.Text;
using CamerackStudio.Models.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CamerackStudio.Models.RabbitMq
{
    public class SendUserMessage
    {
        public void SendNewsLetter(UserEmail email)
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
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "LargeDataEmailTaskManager",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(email);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "LargeDataEmailTaskManager",
                    basicProperties: null,
                    body: body);
            }
        }
        public void SendGeneralNotice(UserEmail email)
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
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "LargeDataEmailTaskManager",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(email);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "LargeDataEmailTaskManager",
                    basicProperties: null,
                    body: body);
            }
        }
    }
}
