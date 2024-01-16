namespace foodBazar.Services.EmailApi.Messaging
{
	public interface IAzureServiceBusConsumer
	{

		Task StartAsync();
		Task StopAsync();
	}
}
