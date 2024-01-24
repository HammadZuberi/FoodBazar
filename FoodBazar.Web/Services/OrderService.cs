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
	}
}
