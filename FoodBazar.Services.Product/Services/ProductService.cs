using AutoMapper;
using Azure;
using FoodBazar.Services.Product.Data;
using FoodBazar.Services.Product.Models;
using FoodBazar.Services.Product.Models.Dto;
using FoodBazar.Services.Product.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace FoodBazar.Services.Product.Services
{
	public class ProductService : IProductService
	{

		private readonly AppDbContext _db;


		private IMapper _mapper;

		private ResponseDto _response;
		public ProductService(AppDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
			_response = new ResponseDto();

		}

		public async Task<ResponseDto> GetAllProducts()
		{
			IEnumerable<Products> products = await _db.Products.ToListAsync();
			_response.Result = _mapper.Map<ProductDto>(products);

			return _response;
		}




		public async Task<ResponseDto> GetProduct(int id)
		{
			try
			{

				Products obj = await _db.Products.FirstOrDefaultAsync(u => u.ProductId == id);
				_response.Result = _mapper.Map<ProductDto>(obj);
			}
			catch (Exception ex)
			{

				_response.Message = ex.Message;
				_response.IsSuccess = false;

			}
			return _response;
		}

		public async Task<ResponseDto> GetProductByName(string Name)
		{
			try
			{

				Products obj = await _db.Products.FirstOrDefaultAsync(u => u.Name.ToLower() == Name.ToLower());
				if (obj == null)
				{
					_response.Message = "Product not found";

				}
				_response.Result = _mapper.Map<ProductDto>(obj);
			}
			catch (Exception ex)
			{

				_response.Message = ex.Message;
				_response.IsSuccess = false;

			}
			return _response;
		}

		public async Task<ResponseDto> CreateProduct(ProductDto model)
		{
			try
			{
				Products products = _mapper.Map<Products>(model);
				await _db.Products.AddAsync(products);
				_db.SaveChanges();
				_response.Message = "Created Successfully";

				_response.Result = _mapper.Map<ProductDto>(products);


			}
			catch (Exception ex)
			{

				_response.Message = ex.Message;
				_response.IsSuccess = false;
			}

			return _response;
		}

		public async Task<ResponseDto> DeleteProduct(int id)
		{
			try
			{
				var product = await _db.Products.FirstOrDefaultAsync(u => u.ProductId == id);
				if (product == null)
				{
					_response.Message = "Product Not Found";
				}
				else
				{
					_db.Products.Remove(product);
					_db.SaveChanges();
					_response.Message = "Deleted Successfully";
				}


			}
			catch (Exception ex)
			{

				_response.Message = ex.Message;
				_response.IsSuccess = false;
			}

			return _response;
		}
		public async Task<ResponseDto> UpdateProduct(ProductDto model)
		{
			try
			{
				Products obj = _mapper.Map<Products>(model);

				_db.Products.Update(obj);

				_db.SaveChanges();
				_response.Result = _mapper.Map<ProductDto>(obj);
			}
			catch (Exception ex)
			{

				_response.Message = ex.Message;
				_response.IsSuccess = false;

			}
			return _response;
		}

	}
}
