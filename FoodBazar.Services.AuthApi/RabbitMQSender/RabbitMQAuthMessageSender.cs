﻿using FoodBazar.Services.AuthApi.RabittMQSender;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace FoodBazar.Services.AuthApi.RabbitMQSender
{
	public class RabbitMQAuthMessageSender : IRabbitMQAuthMessageSender
	{
		private readonly string _hostName;
		private readonly string _userName;
		private readonly string _password;

		private IConnection _connection;
		public RabbitMQAuthMessageSender()
		{
			_hostName = "localhost";
			_userName = "guest";
			_password = "guest";
		}
		public void PublishMessage(string queueName, object message)
		{
			if (ConnectionExists())
			{
				using var channel = _connection.CreateModel();
				channel.QueueDeclare(queueName, false, false, false, null);

				var json = JsonConvert.SerializeObject(message);
				var body = Encoding.UTF8.GetBytes(json);

				channel.BasicPublish(exchange: "", routingKey: queueName, null, body: body);

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
