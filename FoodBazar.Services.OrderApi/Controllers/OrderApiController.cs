using AutoMapper;
using FoodBazar.Services.OrderApi.Data;
using FoodBazar.Services.OrderApi.Models;
using FoodBazar.Services.OrderApi.Models.Dto;
using FoodBazar.Services.OrderApi.Service;
using FoodBazar.Services.OrderApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodBazar.Services.OrderApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrderApiController : ControllerBase
	{
		protected ResponseDto _response;
		private IMapper _mapper;
		private readonly AppDbContext _dbContext;
		private IProductService _productService;

		public OrderApiController(AppDbContext dbContext, IMapper mapper
			, IProductService productService)
		{
			_dbContext = dbContext;
			_mapper = mapper;
			_productService = productService;
			_response = new ResponseDto();

		}

		[HttpPost]
		public async Task<ResponseDto> CreateOrder([FromBody]
		CartDto cart)
		{
			try
			{

				OrderHeaderDto orderHeader = _mapper.Map<OrderHeaderDto>(cart.CartHeader);

				orderHeader.OrderTime = DateTime.Now;
				orderHeader.Status = SD.Status_Pending;
				//orderDetails
				orderHeader.OrderDetails = _mapper
					.Map<IEnumerable<OrderDetailsDto>>(cart.CartDetails);


				// create order  and map dto to model

				OrderHeader orderCreated = _dbContext.OrderHeader.Add(_mapper.Map<OrderHeader>(orderHeader)).Entity;
				await _dbContext.SaveChangesAsync();

				orderHeader.OrderHeaderId = orderCreated.OrderHeaderId;
				_response.Result = orderHeader;
			}
			catch (Exception ex)
			{
				_response.Message = ex.Message.ToString();
				_response.IsSuccess = false;
			}

			return _response;

		}

	}
}
