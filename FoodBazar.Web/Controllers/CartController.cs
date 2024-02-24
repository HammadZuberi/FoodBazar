using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;

namespace FoodBazar.Web.Controllers
{
	//[Authorize]
	public class CartController : Controller
	{
		private readonly ICartService _service;
		private readonly IOrderService _orderservice;

		public CartController(ICartService service, IOrderService orderservice)
		{
			_service = service;
			_orderservice = orderservice;
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> CartIndex()
		{
			return View(await LoadCartBasedonUser());
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Checkout()
		{
			return View("Checkout", await LoadCartBasedonUser());
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> CheckoutAuth()
		{
			return View("Checkout", await LoadCartBasedonUser());
		}

		[HttpPost]
		[ActionName("CheckoutAuth")]
		public async Task<IActionResult> Checkout(CartDto cartDto)
		{
			CartDto cart = await LoadCartBasedonUser();
			try
			{
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
						var domain = Request.Scheme + "://" + Request.Host.Value + "/";

						StripeRequestDto stripeRequest = new()
						{
							ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeader.OrderHeaderId,
							CancelUrl = domain + "cart/Checkout",
							orderHeader = orderHeader,

						};

						var stripeResponse = await _orderservice.CreateStripeSession(stripeRequest);

						StripeRequestDto stripeResult = UtilityHelper
							.DeserializeObject<StripeRequestDto>(stripeResponse.Result);

						Response.Headers.Add("Location", stripeResult.StripeSessionUrl);

						//redirection
						return new StatusCodeResult(303);

					}
					return View("Checkout");
				}
				else
				{

					TempData["error"] = response.Message;
				}
			}
			catch (Exception e)
			{

				TempData["error"] = e.StackTrace;
			}
			return View("Checkout");
		}

		[Authorize]
		public async Task<IActionResult> Confirmation(int orderId)
		{
			//orderId when order genrate a process or service bus can process and confirm all the orders  in order api 

			//validate 
			ResponseDto? response = await _orderservice.ValidateStripeSession(orderId);

			if (response.IsSuccess && response != null)
			{
				OrderHeaderDto orderHeaderDto = UtilityHelper.DeserializeObject<OrderHeaderDto>(response.Result);
				if (orderHeaderDto != null && orderHeaderDto.Status == SD.Status_Approved)
				{
					TempData["success"] = "Order confirmed";
					return View(orderId);

				}
			}
			//redirect ot erro or page based on status
			return View(orderId);

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

				var email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email).FirstOrDefault()?.Value;
				cartDto.CartHeader.Email = email;
				return cartDto;
			}
			return new CartDto();
		}

		[HttpPost]
		public async Task<IActionResult> EmailCart(CartDto cartDto)
		{
			CartDto cart = await LoadCartBasedonUser();

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
