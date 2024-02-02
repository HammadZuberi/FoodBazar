using Azure.Messaging.ServiceBus;
using FoodBazar.Services.RewardsApi.Models.Dto;
using FoodBazar.Services.RewardsApi.Service;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text;

namespace foodBazar.Services.RewardsApi.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly string ServiceBusConnStr;
		private readonly string OrderCreatedTopic;
		private readonly string OrderCreatedRewardsSubscription;
		private readonly IConfiguration _configuration;
		private ServiceBusProcessor _rewardsProcessor;
		private RewardsService _rewardsService;
		public AzureServiceBusConsumer(IConfiguration configuration, RewardsService rewardsService)
		{
			_rewardsService = rewardsService;
			this._configuration = configuration;
			ServiceBusConnStr = _configuration.GetValue<string>("ServiceBusConnectionString");
			OrderCreatedTopic = _configuration.GetValue<string>("topicandQueueNames:OrderCreatedTopic");
			OrderCreatedRewardsSubscription = _configuration.GetValue<string>("topicandQueueNames:OrderCreatedRewards_Subs");


			//create processor to listen to Queue and Topics  for any new msg topic name and subs for topic
			var client = new ServiceBusClient(ServiceBusConnStr);
			_rewardsProcessor = client.CreateProcessor(OrderCreatedTopic, OrderCreatedRewardsSubscription);

		}

		public async Task StartAsync()
		{
			_rewardsProcessor.ProcessMessageAsync += OnnewOrderRewardsRequestReceived;
			_rewardsProcessor.ProcessErrorAsync += ErrorHandler;
			await _rewardsProcessor.StartProcessingAsync();


			//return Task.CompletedTask;
		}



		public async Task StopAsync()
		{
			await _rewardsProcessor.StopProcessingAsync();
			await _rewardsProcessor.DisposeAsync();

		}

		private async Task OnnewOrderRewardsRequestReceived(ProcessMessageEventArgs args)
		{
			var messgae = args.Message;
			var body = Encoding.UTF8.GetString(messgae.Body);

			//async mesg in cart object
			RewardsDto rewards = JsonConvert.DeserializeObject<RewardsDto>(body);
			try
			{
				//TODO -try to log email-

				await _rewardsService.UpdateRewardsinDB(rewards);
				await args.CompleteMessageAsync(args.Message);

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private Task ErrorHandler(ProcessErrorEventArgs args)
		{
			//typically send email when an exception is thrown 
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		}


	}
}
