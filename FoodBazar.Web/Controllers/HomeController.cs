using FoodBazar.Services.Web.Services.IServices;
using FoodBazar.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FoodBazar.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;
		public HomeController(ILogger<HomeController> logger, IProductService product)
		{
			_logger = logger;
			_productService = product;

		}

		public async Task<IActionResult> Index()
		{

			List<ProductDto?> list = new();
			ResponseDto response = await _productService.GetAllProducts();
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));

			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(list);
			//return View();
		}

		[Authorize]
		public async Task<IActionResult> ProductDetails(int productId)
		{

			ProductDto? list = new();
			ResponseDto response = await _productService.GetProduct(productId);
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(list);
			//return View();
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
