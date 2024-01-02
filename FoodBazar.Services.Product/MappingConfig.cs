using AutoMapper;
using FoodBazar.Services.Product.Models;
using FoodBazar.Services.Product.Models.Dto;

namespace FoodBazar.Services.Product
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{

			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<ProductDto, Products>().ReverseMap();
			});

			return mappingConfig;
		}

	}
}
