namespace FoodBazar.Services.AuthApi.RabittMQSender
{
	public interface IRabbitMQMessageSender
	{

		public void PublishMessage(string queueexchangeName, object message, bool isExchange = false);
	}
}
