namespace foodBazar.Services.RewardsApi.Messaging
{
	public interface IAzureServiceBusConsumer
	{

		Task StartAsync();
		Task StopAsync();
	}
}
