using Azure.Messaging.ServiceBus;
using FoodBazar.Services.EmailApi.Models.Dto;
using Newtonsoft.Json;
using System.Text;

namespace foodBazar.Services.EmailApi.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly string ServiceBusConnStr;
		private readonly string emailCartQueue;
		private readonly IConfiguration _configuration;
		private ServiceBusProcessor _emailCartProcessor;
		public AzureServiceBusConsumer(IConfiguration configuration)
		{
			this._configuration = configuration;
			ServiceBusConnStr = _configuration.GetValue<string>("ServiceBusConnectionString");
			emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQ");

			var client = new ServiceBusClient(ServiceBusConnStr);

			//create processor to listen to Queue and Topics  for any new msg
			_emailCartProcessor = client.CreateProcessor(emailCartQueue);


		}

		public async Task StartAsync()
		{
			_emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
			_emailCartProcessor.ProcessErrorAsync += ErrorHandler;
			await _emailCartProcessor.StartProcessingAsync();
			//return Task.CompletedTask;
		}
		public async Task StopAsync()
		{
			await _emailCartProcessor.StopProcessingAsync();
			await _emailCartProcessor.DisposeAsync();
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
