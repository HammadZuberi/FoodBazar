using FoodBazar.Web.Models;

namespace FoodBazar.Web.Services.IServices
{
	public interface IOrderService
	{
		Task <ResponseDto?> CreateOrder(CartDto cart);
	}
}
