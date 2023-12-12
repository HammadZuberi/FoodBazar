using AutoMapper;
using FoodBazar.Services.CouponApi.Model;
using FoodBazar.Services.CouponApi.Model.Dto;

namespace FoodBazar.Services.CouponApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {

            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>().ReverseMap();
            });

            return mappingConfig;
        }

    }
}
