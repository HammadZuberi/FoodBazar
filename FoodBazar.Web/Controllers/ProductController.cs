using FoodBazar.Services.Web.Models.Dto;
using FoodBazar.Services.Web.Services.IServices;
using FoodBazar.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FoodBazar.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _service;

		public ProductController(IProductService service)
		{
			_service = service;

		}
		public async Task<IActionResult> ProductIndex()
		{

			List<ProductDto?> list = new();
			ResponseDto response = await _service.GetAllProducts();
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));

			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(list);
		}

		public async Task<IActionResult> CreateProduct()
		{

			return View();

		}

		public async Task<IActionResult> CreateProduct(ProductDto model)
		{
			if (ModelState.IsValid)
			{
				ResponseDto? response = await _service.CreateProduct(model);
				if (response != null && response.IsSuccess)
				{
					TempData["Success"] = "Coupon Created Successfully";
					return RedirectToAction(nameof(ProductIndex));

				}
				else
				{

					TempData["error"] = response?.Message;
				}
			}

			return View(model);
		}

		public async Task<IActionResult> DeleteProduct(int productId)
		{
			ResponseDto? response = await _service.DeleteProduct(productId);

			if (response != null && response.IsSuccess)
			{

				TempData["Success"] = "Product Deleted Successfully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return NotFound();
		}


		public async Task<IActionResult> EditProduct(int Id)
		{

			ProductDto? product = new();
			ResponseDto response = await _service.GetProduct(Id);
			if (response != null && response.IsSuccess)
			{
				product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(product);

		}

		public async Task<IActionResult> EditProduct(ProductDto model)
		{
			if (ModelState.IsValid)
			{
				ResponseDto? response = await _service.UpdateProduct(model);
				if (response != null && response.IsSuccess)
				{
					TempData["Success"] = "Product Updated Successfully";
					return RedirectToAction(nameof(ProductIndex));

				}
				else
				{

					TempData["error"] = response?.Message;
				}
			}

			return View(model);
		}



	}
}
