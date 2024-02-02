using FoodBazar.Services.RewardsApi.Models.Dto;

namespace FoodBazar.Services.RewardsApi.Service
{
	public interface IRewardsService
	{
		Task<bool> UpdateRewardsinDB(RewardsDto rewardsDto);
	}
}
