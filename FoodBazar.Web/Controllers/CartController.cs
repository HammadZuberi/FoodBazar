using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace FoodBazar.Web.Controllers
{
	public class CartController : Controller
	{
		private readonly ICartService _service;
		private readonly IOrderService _orderservice;

		public CartController(ICartService service, IOrderService orderservice)
		{
			_service = service;
			_orderservice = orderservice;
		}
		public async Task<IActionResult> CartIndex()
		{
			return View(await LoadCartBasedonUser());
		}

		public async Task<IActionResult> Checkout()
		{
			return View(await LoadCartBasedonUser());
		}
		[HttpPost]
		[ActionName("Checkout")]
		public async Task<IActionResult> Checkout(CartDto cartDto)
		{

			CartDto cart = await LoadCartBasedonUser();
			//other user related field from cart dto
			cart.CartHeader.Phone = cartDto.CartHeader.Phone;
			cart.CartHeader.Email = cartDto.CartHeader.Email;
			cart.CartHeader.FirstName = cartDto.CartHeader.FirstName;

			ResponseDto? response = await _orderservice.CreateOrder(cart);

			if (response.IsSuccess && response != null)
			{
				TempData["success"] = "Order created successfully";

				OrderHeaderDto orderHeader = UtilityHelper.DeserializeObject<OrderHeaderDto>(response.Result);
				if (response.IsSuccess && response != null)
				{
					//get stripe session and redirect to place order

				}
				return View();
			}
			else
			{

				TempData["error"] = response.Message;
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
		{
			ResponseDto? response = await _service.ApplyCouponAsync(cartDto);

			if (response.IsSuccess && response != null)
			{
				TempData["success"] = "Cart updated Successfully";
				return RedirectToAction(nameof(CartIndex));
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
		{
			cartDto.CartHeader.CouponCode = "";
			ResponseDto? response = await _service.ApplyCouponAsync(cartDto);

			if (response.IsSuccess && response != null)
			{
				TempData["success"] = "Cart updated Successfully";
				return RedirectToAction(nameof(CartIndex));
			}
			return View();
		}
		[HttpPost]

		public async Task<IActionResult> Remove(int cartDetailsId)
		{
			var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
			ResponseDto? response = await _service.RemoveFromCartAsync(cartDetailsId);

			if (response.IsSuccess && response != null)
			{
				TempData["success"] = "Cart updated Successfully";
				return RedirectToAction(nameof(CartIndex));
			}
			return View();
		}


		public async Task<CartDto> LoadCartBasedonUser()
		{
			var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
			ResponseDto? response = await _service.GetCartbyUserIdAsync(userId);

			if (response.IsSuccess && response != null)
			{
				CartDto cartDto = UtilityHelper.DeserializeObject<CartDto>(response.Result);
				return cartDto;
			}
			return new CartDto();
		}

		[HttpPost]
		public async Task<IActionResult> EmailCart(CartDto cartDto)
		{
			CartDto cart = await LoadCartBasedonUser();

			var email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email).FirstOrDefault()?.Value;


			ResponseDto? response = await _service.EmailCart(cart);

			if (response.IsSuccess && response != null)
			{
				TempData["success"] = "Emial will be processed and sent shortly";
				return RedirectToAction(nameof(CartIndex));
			}
			return View();
		}
	}
}
