using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;

namespace FoodBazar.Web.Services
{
	public class CartService : ICartService
    {
        private IHttpClientFactory _httpClientFactory;
        private IBaseService _baseService;
        public CartService(IHttpClientFactory httpClientFactory, IBaseService baseService)
        {
            _httpClientFactory = httpClientFactory;
            this._baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.CartApiUri + "/api/cart/ApplyCoupon"
            });
        }

		public async Task<ResponseDto?> EmailCart(CartDto cartDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = cartDto,
				Url = SD.CartApiUri + "/api/cart/EmailCartRequest"
			});
		}

		public async Task<ResponseDto?> GetCartbyUserIdAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CartApiUri + $"/api/cart/GetCart/{userId}"
            });
        }

        public  async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDetailsId,
                Url = SD.CartApiUri + "/api/cart/RemoveCart"
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.CartApiUri + "/api/cart/Cartupsert"
            });
        }
    }
}
