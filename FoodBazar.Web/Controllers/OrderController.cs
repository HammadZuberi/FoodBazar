using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace FoodBazar.Web.Controllers
{
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public IActionResult OrderIndex()
		{
			return View();
		}

		public async Task<IActionResult> OrderDetail(int orderId)
		{

			OrderHeaderDto order = new OrderHeaderDto();

			string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

			ResponseDto response = await _orderService.GetOder(orderId);
			if (response.IsSuccess && response != null)
			{
				order = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
			}
			if ((!User.IsInRole(SD.RoleAdmin)) && userId != order.UserId)
			{//if not admin or try another user order
				return NotFound();
			}
			return View(order);
		}

		[HttpPost("OrderReadyforPickup")]
		public async Task<IActionResult> OrderReadyforPickup(int orderId)
		{

			ResponseDto response = await _orderService.UpdateOrderStatus(orderId, SD.Status_ReadyForPickUp);
			if (response.IsSuccess && response != null)
			{
				TempData["success"] = "Order updated Successfully";
				return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
			}

			return View();

		}

		[HttpPost("CompleteOrder")]
		public async Task<IActionResult> CompleteOrder(int orderId)
		{

			ResponseDto response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Completed);
			if (response.IsSuccess && response != null)
			{
				TempData["success"] = "Order updated Successfully";
				return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
			}

			return View();

		}

		[HttpPost("CancelOrder")]
		public async Task<IActionResult> CancelOrder(int orderId)
		{

			ResponseDto response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);
			if (response.IsSuccess && response != null)
			{
				TempData["success"] = "Order updated Successfully";
				return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
			}

			return View();

		}
		[HttpGet]
		public IActionResult GetAll(string status)
		{
			IEnumerable<OrderHeaderDto> list;
			string userId = "";
			if (!User.IsInRole(SD.RoleAdmin))
			{
				userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
			}

			ResponseDto response = _orderService.GetAllOrders(userId).GetAwaiter().GetResult();

			if (response.IsSuccess && response != null)
			{
				list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));

				switch (status)
				{
					case "approved":
						list = list.Where(u => u.Status == SD.Status_Approved); break;

					case "readyforpickup":
						list = list.Where(u => u.Status == SD.Status_ReadyForPickUp); break;

					case "cancelled":
						list = list.Where(u => u.Status == SD.Status_Cancelled || u.Status == SD.Status_Refunded ); break;
						default
						: break;	

				}
			}
			else
			{
				list = new List<OrderHeaderDto>();
			}
			return Json(new { data = list });
		}
	}
}
