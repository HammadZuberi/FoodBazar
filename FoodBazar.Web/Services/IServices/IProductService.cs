
using FoodBazar.Services.Web.Models.Dto;
using FoodBazar.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodBazar.Services.Web.Services.IServices
{
	public interface IProductService
	{
		public Task<ResponseDto?> GetAllProducts();
		public Task<ResponseDto?> GetProduct(int id);
		public Task<ResponseDto?> GetProductByName(string Name);
		public Task<ResponseDto?> CreateProduct(ProductDto model);
		public Task<ResponseDto?> UpdateProduct(ProductDto model);
		public Task<ResponseDto?> DeleteProduct(int id);
	}
}
