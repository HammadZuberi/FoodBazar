using foodBazar.Services.EmailApi.Messaging;

namespace foodBazar.Services.EmailApi.Extension
{
	public static class AppplicationBuilderExtension
	{
		private static IAzureServiceBusConsumer azureBusConsumer { get; set; }
		public static IApplicationBuilder UseAzureServiceBusCoinsumer(this IApplicationBuilder app)
		{
			azureBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
			//notify when start and stop
			var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

			hostApplicationLife.ApplicationStarted.Register(OnStart);
			hostApplicationLife.ApplicationStarted.Register(OnStop);
			//return to continue pipeline;
			return app;
		}

		private static void OnStart()
		{
			azureBusConsumer.StartAsync();
		}

		private static void OnStop()
		{
			azureBusConsumer.StopAsync();
		}
	}
}
