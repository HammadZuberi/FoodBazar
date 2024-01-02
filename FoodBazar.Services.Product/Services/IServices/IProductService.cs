using FoodBazar.Services.Product.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FoodBazar.Services.Product.Services.IServices
{
	public interface IProductService
	{
		public Task<ResponseDto> GetAllProducts();
		public Task<ResponseDto> GetProduct(int id);
		public Task<ResponseDto> GetProductByName(string Name);
		public Task<ResponseDto> CreateProduct(ProductDto model);
		public Task<ResponseDto> UpdateProduct(ProductDto model);
		public Task<ResponseDto> DeleteProduct(int id);
	}
}
