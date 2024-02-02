using foodBazar.Services.EmailApi.Models;
using foodBazar.Services.EmailApi.Models.Dto;
using FoodBazar.Services.EmailApi.Data;
using FoodBazar.Services.EmailApi.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace foodBazar.Services.EmailApi.Service
{
	public class EmailService : IEmailService
	{
		private DbContextOptions<AppDbContext> _appDbOptions;
		public EmailService(DbContextOptions<AppDbContext> appDbOptions)
		{
			this._appDbOptions = appDbOptions;

		}

		public async Task EmailCartandLog(CartDto cart)
		{
			StringBuilder message = new StringBuilder();

			message.AppendLine("</br> Cart Email Requested");
			message.AppendLine("<br/>Total" + cart.CartHeader.CartTotal);
			message.AppendLine("<br/>");
			message.AppendLine("<ul>");
			foreach (var item in cart.CartDetails)
			{

				message.AppendLine("<li>");
				message.AppendLine(item.Product.Name + "x" + item.Count);
				message.AppendLine("</li>");
			}
			message.Append("</ul>");

			await LogandEmail(message.ToString(), cart.CartHeader.Email);
		}
		public async Task EmailUserRegistered(string email)
		{
			StringBuilder message = new StringBuilder();

			message.AppendLine("</br> User Registeration Requested");
			message.AppendLine("<br/> User :" + email + "Has been registered ");
			message.AppendLine("<br/> with the Folowing Email" + email);

			await LogandEmail(message.ToString(), email);
		}

		public async Task LogOrderPlaced(RewardsDto rewardsDto)
		{
			string email = "";
			StringBuilder message = new StringBuilder();

			message.AppendLine("</br> Order Confirmation");
			message.AppendLine("<br/> Order Placed :" + rewardsDto.OrderId + "");
			//message.AppendLine("<br/> with the Folowing Email" + email);

			await LogandEmail(message.ToString(), email);
		}

		private async Task<bool> LogandEmail(string message, string email)
		{
			try
			{
				EmailLogger email1 = new EmailLogger() { Email = email, EmailSent = DateTime.Now, Message = message };
				//store in Db give us object of db option in the context
				await using var _db = new AppDbContext(_appDbOptions);
				await _db.EmailLoggers.AddAsync(email1);
				await _db.SaveChangesAsync();

				return true;

			}
			catch (Exception ex)
			{

				return false;
			}
		}
	}
}
