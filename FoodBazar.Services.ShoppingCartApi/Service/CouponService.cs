using FoodBazar.Services.ShoppingCartApi.Model.Dto;
using FoodBazar.Services.ShoppingCartApi.Models.Dto;
using Newtonsoft.Json;

namespace FoodBazar.Services.ShoppingCartApi.Service
{
    public interface ICouponService
    {
        public Task<CouponDto> GetCoupon(string couponcode);
    }
    public class CouponService : ICouponService
    {


        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }
        public async Task<CouponDto> GetCoupon(string couponcode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");

            var response = await client.GetAsync($"api/Coupon/GetCode/{couponcode}");
            var apicontent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apicontent);
            if (resp != null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));

            }

            return new CouponDto();
        }
    }
}
