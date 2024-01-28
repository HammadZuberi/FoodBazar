using FoodBazar.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodBazar.Web.Services.IServices
{
	public interface IOrderService
	{
		Task <ResponseDto?> CreateOrder(CartDto cart);
		Task <ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequest);
		 Task<ResponseDto> ValidateStripeSession(int orderHeaderId);
	}
}
