using FoodBazar.Services.RewardsApi.Models.Dto;
using FoodBazar.Services.RewardsApi.Service;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace foodBazar.Services.RewardsApi.Messaging
{
	public class RabbitMQConsumer : BackgroundService
	{


		private readonly IConfiguration _configuration;
		private IRewardsService _rewardsService;
		private IConnection _connection;
		private IModel _channel;
		private string OrderQueue;
		private string queueName = "";

		public RabbitMQConsumer(IConfiguration configuration, IRewardsService rewardsService)
		{
			_configuration = configuration;
			_rewardsService = rewardsService;


			var factory = new ConnectionFactory
			{
				HostName = "localhost",
				UserName = "guest",
				Password = "guest"
			};

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();

			OrderQueue = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
			//fanout exchange
			_channel.ExchangeDeclare(OrderQueue, ExchangeType.Fanout);
			//creates a default queue fetch from Rabbit Mq
			queueName = _channel.QueueDeclare().QueueName;
			_channel.QueueBind(queueName, OrderQueue, "");
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
			_rewardsService.UpdateRewardsinDB(rewards).GetAwaiter().GetResult();
		}
	}
}
