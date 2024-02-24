using foodBazar.Services.EmailApi.Models;
using foodBazar.Services.EmailApi.Service;
using FoodBazar.Services.EmailApi.Models.Dto;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace foodBazar.Services.EmailApi.Messaging
{

	public class RMQEmailConsumer : BackgroundService
	{
		private readonly IConfiguration _configuration;

		private EmailService _emailService;
		private IModel _channel;
		private string EmailQueue;
		public RMQEmailConsumer(IConfiguration configuration, EmailService emailService)
		{
			_configuration = configuration;
			_emailService = emailService;

			RabbitMQConnection cq = new RabbitMQConnection();
			_channel = cq.GetChannel();



			EmailQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQ");
			_channel.QueueDeclare(EmailQueue, false, false, false, null);
		}



		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);
			//create vent handler
			consumer.Received += (chnl, evnt) =>
			{
				var content = Encoding.UTF8.GetString(evnt.Body.ToArray());
				CartDto cart = JsonConvert.DeserializeObject<CartDto>(content);
				HandleMessage(cart).GetAwaiter().GetResult();

				_channel.BasicAck(evnt.DeliveryTag, false);
			};

			_channel.BasicConsume(EmailQueue, false, consumer);

			return Task.CompletedTask;
		}

		private async Task HandleMessage(CartDto cart)
		{
			_emailService.EmailCartandLog(cart).GetAwaiter().GetResult();
		}
	}
}
