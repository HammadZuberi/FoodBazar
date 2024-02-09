using FoodBazar.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodBazar.Web.Services.IServices
{
	public interface IOrderService
	{
		Task<ResponseDto?> CreateOrder(CartDto cart);
		Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequest);
		Task<ResponseDto?> ValidateStripeSession(int orderHeaderId);
		Task<ResponseDto?> GetAllOrders(string? userId);
		Task<ResponseDto?> GetOder(int orderHeaderId);
		Task<ResponseDto?> UpdateOrderStatus(int orderHeaderId,string newStatus);
	}
}
