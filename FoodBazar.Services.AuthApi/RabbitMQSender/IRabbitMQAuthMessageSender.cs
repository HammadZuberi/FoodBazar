namespace FoodBazar.Services.AuthApi.RabittMQSender
{
	public interface IRabbitMQAuthMessageSender
	{

		public void PublishMessage(string queueName,object message);
	}
}
