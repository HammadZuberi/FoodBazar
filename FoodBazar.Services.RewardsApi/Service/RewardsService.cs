using FoodBazar.Services.RewardsApi.Data;
using FoodBazar.Services.RewardsApi.Models;
using FoodBazar.Services.RewardsApi.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FoodBazar.Services.RewardsApi.Service
{

	public class RewardsService : IRewardsService
	{
		private readonly DbContextOptions<AppDbContext> _dbContext;
		public RewardsService(DbContextOptions<AppDbContext> dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<bool> UpdateRewardsinDB(RewardsDto rewardsDto)
		{
			try
			{

				Rewards rewards = new Rewards()
				{
					OrderId = rewardsDto.OrderId,
					RewardsActivity = rewardsDto.RewardsActivity,
					UserId = rewardsDto.UserId,
					RewardsDate = DateTime.Now
				};

				await using var _db = new AppDbContext(_dbContext);
				await _db.Rewards.AddAsync(rewards);
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
