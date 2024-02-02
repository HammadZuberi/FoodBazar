using AutoMapper;
using FoodBazar.Services.RewardsApi.Models;
using FoodBazar.Services.RewardsApi.Models.Dto;

namespace FoodBazar.Services.RewardsApi
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{

			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Rewards, RewardsDto>().ReverseMap();
			});

			return mappingConfig;
		}

	}
}
