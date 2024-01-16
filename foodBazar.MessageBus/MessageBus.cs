using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace foodBazar.MessageBus
{

	public class MessageBus : IMessageBus
	{
		private string ConncetionString = "";
		public async Task PublishMessage(string Topic_queue_Name, object message)
		{
			await using var client = new ServiceBusClient(ConncetionString);

			ServiceBusSender sender = client.CreateSender(Topic_queue_Name);

			var Jsonmsg = JsonConvert.SerializeObject(message);
			ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(Jsonmsg))
			{
				CorrelationId = Guid.NewGuid().ToString(),
			};

			await sender.SendMessageAsync(finalMessage);

			await client.DisposeAsync();
		}
	}
}
