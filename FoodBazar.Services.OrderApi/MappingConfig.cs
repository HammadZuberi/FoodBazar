using AutoMapper;
using FoodBazar.Services.OrderApi.Models;
using FoodBazar.Services.OrderApi.Models.Dto;

namespace FoodBazar.Services.OrderApi
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{

			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
				config.CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();

				config.CreateMap<OrderHeaderDto, CartHeaderDto>().ForMember(d => d.CartTotal,
					u => u.MapFrom(s => s.OrderTotal)).ReverseMap();

				config.CreateMap<CartDetailsDto, OrderDetailsDto>()
				.ForMember(d => d.ProductName, u => u.MapFrom(s => s.Product.Name))
				.ForMember(d => d.Price, u => u.MapFrom(s => s.Product.Price));


				config.CreateMap<OrderDetailsDto, CartDetailsDto>();
			});

			return mappingConfig;
		}

	}
}
