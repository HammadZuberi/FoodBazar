using FoodBazar.Services.Web.Models.Dto;
using FoodBazar.Services.Web.Services.IServices;
using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;
using System.Reflection;
using System.Xml.Linq;

namespace FoodBazar.Web.Services
{
	public class ProductService : IProductService
	{

		private readonly IBaseService _baseService;
		public ProductService(IBaseService baseService)
		{
			_baseService = baseService;

		}

		public async Task<ResponseDto?> GetAllProducts()
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Utilities.SD.ApiType.GET,
				Url = SD.ProductApiUri + "/api/product"
			});
		}

		public async Task<ResponseDto?> GetProduct(int id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Utilities.SD.ApiType.GET,
				Url = SD.ProductApiUri + "/api/product/" + id
			});
		}

		public async Task<ResponseDto?> GetProductByName(string Name)
		{

			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Utilities.SD.ApiType.GET,
				Url = SD.ProductApiUri + "/api/product/GetName/" + Name
			});
		}

		public async Task<ResponseDto?> CreateProduct(ProductDto model)
		{

			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Utilities.SD.ApiType.POST,
				Url = SD.ProductApiUri + "/api/product",
				Data = model,
				ContentType= SD.ContentType.MultipartFormData

			});
		}


		public async Task<ResponseDto?> UpdateProduct(ProductDto model)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Utilities.SD.ApiType.PUT,
				Url = SD.ProductApiUri + "/api/product",
				Data = model,
                ContentType = SD.ContentType.MultipartFormData
            });
		}

		public async Task<ResponseDto?> DeleteProduct(int id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Utilities.SD.ApiType.DELETE,
				Url = SD.ProductApiUri + "/api/product/" + id
			});
		}
	}
}
