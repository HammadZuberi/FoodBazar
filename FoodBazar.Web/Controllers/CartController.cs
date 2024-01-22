using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FoodBazar.Web.Controllers
{
	public class CartController : Controller
	{
		private ICartService _service;

		public CartController(ICartService service)
		{
			_service = service;

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
