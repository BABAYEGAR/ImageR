using System.Text;
using CamerackStudio.Models.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CamerackStudio.Models.RabbitMq
{
    public class SendImageMessage
    {
        public void SendImageCreationMessage(ImageUpload upload)
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
                channel.QueueDeclare(queue: "ImageUpload",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(upload);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "ImageUpload",
                    basicProperties: null,
                    body: body);
            }
        }
        public void SendImageRejectionEmailMessage(AppUserTransport appUser)
        {
            appUser.RequestType = "ImageRejection";
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
                channel.QueueDeclare(queue: "EmailTaskManager",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(appUser);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "EmailTaskManager",
                    basicProperties: null,
                    body: body);
            }
        }

    }
}
