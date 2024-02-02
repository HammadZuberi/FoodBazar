using Azure.Messaging.ServiceBus;
using foodBazar.Services.EmailApi.Service;
using FoodBazar.Services.EmailApi.Models.Dto;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text;

namespace foodBazar.Services.EmailApi.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly string ServiceBusConnStr;
		private readonly string emailCartQueue;
		private readonly string RegisrtartionQueue;
		private readonly string OrderCreated_Topic;
		private readonly string OrderCreated_subscription;
		private readonly IConfiguration _configuration;
		private ServiceBusProcessor _emailOrderProcessor;
		private ServiceBusProcessor _emailCartProcessor;
		private ServiceBusProcessor _emailregisterProcessor;
		private EmailService _emailService;
		public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
		{
			_emailService=emailService;
			this._configuration = configuration;
			ServiceBusConnStr = _configuration.GetValue<string>("ServiceBusConnectionString");
			emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQ");
			RegisrtartionQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailRegistrationQ");
			OrderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated");
			OrderCreated_subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedRewardsEmail");

			var client = new ServiceBusClient(ServiceBusConnStr);

			//create processor to listen to Queue and Topics  for any new msg
			_emailCartProcessor = client.CreateProcessor(emailCartQueue);
			_emailregisterProcessor = client.CreateProcessor(RegisrtartionQueue);
			_emailOrderProcessor = client.CreateProcessor(OrderCreated_Topic,OrderCreated_subscription);

		}

		public async Task StartAsync()
		{
			_emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
			_emailCartProcessor.ProcessErrorAsync += ErrorHandler;
			await _emailCartProcessor.StartProcessingAsync();


			_emailregisterProcessor.ProcessMessageAsync += onUserRegisterRequestRecived;
			_emailregisterProcessor.ProcessErrorAsync += ErrorHandler;
			await _emailregisterProcessor.StartProcessingAsync();
			//return Task.CompletedTask;

			_emailOrderProcessor.ProcessMessageAsync += onOrderCreatedRequestRecived;
			_emailOrderProcessor.ProcessErrorAsync += ErrorHandler;
			await _emailOrderProcessor.StartProcessingAsync();
		}

		
		public async Task StopAsync()
		{
			await _emailCartProcessor.StopProcessingAsync();
			await _emailCartProcessor.DisposeAsync();


			await _emailregisterProcessor.StopProcessingAsync();
			await _emailregisterProcessor.DisposeAsync();

			await _emailOrderProcessor.StopProcessingAsync();
			await _emailOrderProcessor.DisposeAsync();
		}
		private async Task onUserRegisterRequestRecived(ProcessMessageEventArgs args)
		{
			var messgae = args.Message;
			var body = Encoding.UTF8.GetString(messgae.Body);

			//async mesg in cart object
			string email = JsonConvert.DeserializeObject<string>(body);
			try
			{
				//TODO -try to log email-

				await _emailService.EmailUserRegistered(email);
				await args.CompleteMessageAsync(args.Message);

			}
			catch (Exception ex)
			{
				throw;
			}
		}
		private async Task onOrderCreatedRequestRecived(ProcessMessageEventArgs args)
		{
			var messgae = args.Message;
			var body = Encoding.UTF8.GetString(messgae.Body);

			//async mesg in cart object
			RewardsDto objMessage = JsonConvert.DeserializeObject<RewardsDto>(body);
			try
			{
				//TODO -try to log email-

				await _emailService.LogOrderPlaced(objMessage);
				await args.CompleteMessageAsync(args.Message);

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
		{
			var messgae = args.Message;
			var body = Encoding.UTF8.GetString(messgae.Body);

			//async mesg in cart object
			CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(body);
			try
			{
				//TODO -try to log email-

				await _emailService.EmailCartandLog(cartDto);
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
