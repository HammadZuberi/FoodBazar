using FoodBazar.Services.AuthApi.RabittMQSender;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace FoodBazar.RabbitMQSender
{
	public class RabbitMQMessageSender : IRabbitMQMessageSender
	{
		private readonly string _hostName;
		private readonly string _userName;
		private readonly string _password;

		private IConnection _connection;
		public RabbitMQMessageSender()
		{
			_hostName = "localhost";
			_userName = "guest";
			_password = "guest";
		}
		public void PublishMessage(string queueexchangeName, object message, bool isExchange = false)
		{
			if (ConnectionExists())
			{
				using var channel = _connection.CreateModel();
				if (isExchange)
					channel.ExchangeDeclare(queueexchangeName, ExchangeType.Fanout, durable: false);
				else
					channel.QueueDeclare(queueexchangeName, false, false, false, null);

				var json = JsonConvert.SerializeObject(message);
				var body = Encoding.UTF8.GetBytes(json);

				if (isExchange)
					channel.BasicPublish(exchange: queueexchangeName, "", null, body: body);
				else
					channel.BasicPublish(exchange: "", routingKey: queueexchangeName, null, body: body);

			}

		}


		public void CreateConnection()
		{
			try
			{
				var factory = new ConnectionFactory
				{
					HostName = _hostName,
					Password = _password,
					UserName = _userName

				};

				_connection = factory.CreateConnection();
			}
			catch (Exception ex)
			{

			}
		}

		public bool ConnectionExists()
		{
			if (_connection != null)
			{
				return true;
			}
			CreateConnection();
			return true;
		}
	}
}
