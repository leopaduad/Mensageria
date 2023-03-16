using Mensageria.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mensageria
{
    public static class RabbitMqPublish
    {
        public static async void EnviarMensagem(Funcionario escala)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(@"amqp://guest:guest@127.0.0.1:5672/"),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "EscalaFuncionarios",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "Gleidsinho muleque doido2!";
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(escala));

                channel.BasicPublish(exchange: "",
                                     routingKey: "EscalaFuncionarios",
                                     basicProperties: null,
                                     body: body);
            }            
        }
    }
}