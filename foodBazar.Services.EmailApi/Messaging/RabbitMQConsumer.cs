using foodBazar.Services.EmailApi.Service;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace foodBazar.Services.EmailApi.Messaging
{
	public class RabbitMQConsumer : BackgroundService
	{


		private readonly IConfiguration _configuration;
		private EmailService _emailService;
		private IConnection _connection;
		private IModel _channel;
		private string RegisrtartionQueue;

		public RabbitMQConsumer(IConfiguration configuration, EmailService emailService)
		{
			_configuration = configuration;
			_emailService = emailService;


			var factory = new ConnectionFactory
			{
				HostName = "loaclhost",
				UserName = "guest",
				Password = "guest"
			};

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();

			RegisrtartionQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailRegistrationQ");
			_channel.QueueDeclare(RegisrtartionQueue, false, false, false, null);

			//var json = JsonConvert.SerializeObject(message);
			//var body = Encoding.UTF8.GetBytes(json);

			//channel.BasicPublish(exchange: "", routingKey: queueName, null, body: body);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);
			//create vent handler
			consumer.Received += (chnl, evnt) =>
			{
				var content = Encoding.UTF8.GetString(evnt.Body.ToArray());
				string email = JsonConvert.DeserializeObject<string>(content);
				HandleMessage(email).GetAwaiter().GetResult();

				_channel.BasicAck(evnt.DeliveryTag, false);
			};

			_channel.BasicConsume(RegisrtartionQueue, false, consumer);

			return Task.CompletedTask;
		}

		private async Task HandleMessage(string email)
		{
			_emailService.EmailUserRegistered(email).GetAwaiter().GetResult();
		}
	}
}
