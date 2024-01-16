using FoodBazar.Services.Web.Services.IServices;
using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace FoodBazar.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;
		private ICartService _cartService;
		public HomeController(ILogger<HomeController> logger, IProductService product, ICartService cartService)
		{
			_logger = logger;
			_cartService = cartService;
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
		public async Task<IActionResult> ProductDetail(int productId)
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

		[HttpPost]
		[Authorize]
		[ActionName("ProductDetail")]
		public async Task<IActionResult> ProductDetails(ProductDto productDto)
		{

			CartDto cartDto = new CartDto()
			{
				CartHeader = new CartHeaderDto()
				{
					UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
				}
			};

			CartDetailsDto cartDetail = new CartDetailsDto()
			{
				ProductId = productDto.ProductId,
				Count = productDto.Count,
			};

			List<CartDetailsDto> cartDetailsDtos = new() { cartDetail };
			cartDto.CartDetails = cartDetailsDtos;

			ResponseDto response = await _cartService.UpsertCartAsync(cartDto);
			if (response != null && response.IsSuccess)
			{
				//list = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

				TempData["Success"] = "Item has been successfully added to the Cart";
				return RedirectToAction(nameof(Index));

			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(productDto);
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
