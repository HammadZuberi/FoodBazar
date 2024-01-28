using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;

namespace FoodBazar.Web.Services
{
	public class OrderService : IOrderService
	{
		private readonly IBaseService _baseService;
		public OrderService(IBaseService baseService)
		{
			_baseService = baseService;

		}

		public async Task<ResponseDto?> CreateOrder(CartDto cart)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = cart,
				Url = SD.OrderApiUri + "/api/order/CreateOrder"
			});
		}

		public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequest)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = stripeRequest,
				Url = SD.OrderApiUri + "/api/order/CreateStripeSession"
			});
		}

		public async Task<ResponseDto> ValidateStripeSession(int orderHeaderId)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = orderHeaderId,
				Url = SD.OrderApiUri + "/api/order/ValidateStripeSession"
			});
		}
	}
}
