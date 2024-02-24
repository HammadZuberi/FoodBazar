using foodBazar.Services.EmailApi.Models;
using foodBazar.Services.EmailApi.Service;
using FoodBazar.Services.EmailApi.Models.Dto;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace foodBazar.Services.EmailApi.Messaging
{

	public class RMQOrderConsumer : BackgroundService
	{
		private readonly IConfiguration _configuration;

		private EmailService _emailService;
		private IModel _channel;
		private string EmailQueue;
		private string queueName = "";
		public RMQOrderConsumer(IConfiguration configuration, EmailService emailService)
		{
			_configuration = configuration;
			_emailService = emailService;

			RabbitMQConnection cq = new RabbitMQConnection();
			_channel = cq.GetChannel();



			EmailQueue = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
			//fanout exchange
			_channel.ExchangeDeclare(EmailQueue, ExchangeType.Fanout);
			//creates a default queue fetch from Rabbit Mq and bind to exchange
			queueName = _channel.QueueDeclare().QueueName;
			_channel.QueueBind(queueName, EmailQueue, "");
		}



		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);
			//create vent handler
			consumer.Received += (chnl, evnt) =>
			{
				var content = Encoding.UTF8.GetString(evnt.Body.ToArray());
				RewardsDto rewards = JsonConvert.DeserializeObject<RewardsDto>(content);
				HandleMessage(rewards).GetAwaiter().GetResult();

				_channel.BasicAck(evnt.DeliveryTag, false);
			};

			_channel.BasicConsume(queueName, false, consumer);

			return Task.CompletedTask;
		}

		private async Task HandleMessage(RewardsDto rewards)
		{
			_emailService.LogOrderPlaced(rewards).GetAwaiter().GetResult();
		}
	}
}
