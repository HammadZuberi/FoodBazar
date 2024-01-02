using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FoodBazar.Web.Controllers
{
    public class CouponController : Controller
	{
		private readonly ICouponService _couponService;
		public CouponController(ICouponService couponService)
		{
			this._couponService = couponService;

		}

		public async Task<IActionResult> CouponIndex()
		{

			List<CouponDto>? list = new();
			ResponseDto? response = await _couponService.GetAllCouponAsync();

			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));

			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(list);
		}

		public async Task<IActionResult> CreateCoupon()
		{

			return View();

		}

		[HttpPost]
		public async Task<IActionResult> CreateCoupon(CouponDto model)
		{
			if (ModelState.IsValid)
			{
				ResponseDto? response = await _couponService.CreateCouponAsync(model);

				if (response != null && response.IsSuccess)
				{

					TempData["Success"] = "Coupon Created Successfully";
					return RedirectToAction(nameof(CouponIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}

			}
			return View(model);
		}

		public async Task<IActionResult> DeleteCoupon(int couponId)
		{
			ResponseDto? response = await _couponService.DeleteCouponAsync(couponId);

			if (response != null && response.IsSuccess)
			{

				TempData["Success"] = "Coupon Deleted Successfully";
				return RedirectToAction(nameof(CouponIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return NotFound();
		}

		//[HttpPost]
		//public async Task<IActionResult> DeleteCoupon(CouponDto couponDto)
		//{
		//	ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);

		//	if (response != null && response.IsSuccess)
		//	{
		//		return RedirectToAction(nameof(CouponIndex));
		//	}
		//	else
		//	{
		//		TempData["error"] = response?.Message;
		//	}

		//	return View(couponDto);
		//}




	}

}
