using AutoMapper;
using FoodBazar.Services.ShoppingCartApi.Models;
using FoodBazar.Services.ShoppingCartApi.Models.Dto;

namespace FoodBazar.Services.ShoppingCartApi
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{

			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
			});

			return mappingConfig;
		}

	}
}
