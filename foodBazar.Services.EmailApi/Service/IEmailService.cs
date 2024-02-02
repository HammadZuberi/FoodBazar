using FoodBazar.Services.EmailApi.Models.Dto;

namespace foodBazar.Services.EmailApi.Service
{
	public interface IEmailService
	{

		Task EmailCartandLog(CartDto cart);
		Task EmailUserRegistered(string email);
		Task LogOrderPlaced(RewardsDto rewardsDto);
	}
}
