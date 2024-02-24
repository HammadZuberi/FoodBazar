using RabbitMQ.Client;

namespace foodBazar.Services.EmailApi.Messaging
{
	public class RabbitMQConnection
	{
		private IConnection _connection;
		private string _hostName;
		private string _password;
		private string _userName;
		private IModel _channel;


		public RabbitMQConnection()
		{
			_hostName = "localhost";
			_userName = "guest";
			_password = "guest";
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

		private bool ConnectionExists()
		{
			if (_connection != null)
			{
				return true;
			}
			CreateConnection();
			return true;
		}



		public IModel GetChannel()
		{
			if (ConnectionExists())
			{


				return _connection.CreateModel();
			}
			return null;
		}
	}

}