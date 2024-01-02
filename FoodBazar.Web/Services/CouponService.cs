using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;

namespace FoodBazar.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {

            _baseService = baseService;

        }


        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiUri + "/api/coupon"

            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiUri + "/api/coupon/" + id

            });
        }

        public async Task<ResponseDto?> GetCouponCodeAsync(string code)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiUri + "/api/coupon/GetByCode/" + code

            });
        }
        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {

            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.CouponApiUri + "/api/coupon",
                Data = couponDto

            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Url = SD.CouponApiUri + "/api/coupon",
                Data = couponDto

            });
        }


        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponApiUri + "/api/coupon/" + id

            });
        }
    }
}
