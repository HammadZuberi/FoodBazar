using AutoMapper;
using foodBazar.MessageBus;
using FoodBazar.Services.ShoppingCartApi.Data;
using FoodBazar.Services.ShoppingCartApi.Model.Dto;
using FoodBazar.Services.ShoppingCartApi.Models;
using FoodBazar.Services.ShoppingCartApi.Models.Dto;
using FoodBazar.Services.ShoppingCartApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Reflection.PortableExecutable;

namespace FoodBazar.Services.ShoppingCartApi.Controllers
{
	[Route("api/cart")]
	[ApiController]
	//[Authorize]
	public class ShoppingCartApiController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly IMessageBus _messageBus;
		private IMapper _mapper;
		private ResponseDto _responseDto;
		private IConfiguration _config;
		private IProductService _productService;
		private ICouponService _couponService;

		public ShoppingCartApiController(IMapper apper, AppDbContext dbContext,
			IProductService productService, ICouponService couponService, IMessageBus messageBus
			, IConfiguration config)

		{
			_mapper = apper;
			_db = dbContext;
			_productService = productService;
			_responseDto = new ResponseDto();
			_couponService = couponService;
			_messageBus = messageBus;
			_config = config;
		}


		[HttpGet("GetCart/{userId}")]
		public async Task<ResponseDto> GetCart(string userId)
		{
			try
			{
				CartDto cart = new CartDto()
				{
					CartHeader = _mapper.Map<CartHeaderDto>(
						_db.cartHeaders
						.First(c => c.UserId == userId)
						)
				};

				cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(
					_db.cartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId)
					);

				IEnumerable<ProductDto> products = await _productService.GetProductsAsync();
				foreach (var item in cart.CartDetails)
				{
					item.Product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
					cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
				}

				//apply coupon 
				if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
				{
					CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
					if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
					{
						cart.CartHeader.CartTotal -= coupon.DiscountAmount;
						cart.CartHeader.Discount = coupon.DiscountAmount;

					}
				}

				_responseDto.Result = cart;
			}
			catch (Exception ex)
			{

				_responseDto.Message = ex.Message.ToString();
				_responseDto.IsSuccess = false;
			}

			return _responseDto;



		}



		[HttpPost("ApplyCoupon")]
		public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cartDto)
		{
			try
			{

				var cartDb = await _db.cartHeaders
					.FirstOrDefaultAsync(c => c.UserId == cartDto.CartHeader.UserId);

				cartDb.CouponCode = cartDto.CartHeader.CouponCode;

				_db.cartHeaders.Update(cartDb);

				await _db.SaveChangesAsync();
				_responseDto.Result = true;
			}
			catch (Exception ex)
			{

				_responseDto.Message = ex.Message.ToString();
				_responseDto.IsSuccess = false;
			}

			return _responseDto;

		}
		[HttpPost("RemoveCoupon")]
		public async Task<object> RemoveCoupon([FromBody] CartDto cartDto)
		{
			try
			{

				var cartDb = await _db.cartHeaders
					.FirstOrDefaultAsync(c => c.UserId == cartDto.CartHeader.UserId);

				cartDb.CouponCode = "";

				_db.cartHeaders.Update(cartDb);

				await _db.SaveChangesAsync();
				_responseDto.Result = true;
			}
			catch (Exception ex)
			{

				_responseDto.Message = ex.Message.ToString();
				_responseDto.IsSuccess = false;
			}

			return _responseDto;

		}

		[HttpPost("Cartupsert")]
		public async Task<ResponseDto> Upsert([FromBody] CartDto cartDto)
		{
			try
			{
				var cartHeaderDb = await _db.cartHeaders.AsNoTracking()
					.FirstOrDefaultAsync(c => c.UserId == cartDto.CartHeader.UserId);

				if (cartHeaderDb == null)
				{
					//create cart header and details
					CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
					_db.cartHeaders.Add(cartHeader);
					await _db.SaveChangesAsync();

					cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
					_db.cartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
					await _db.SaveChangesAsync();
				}
				else
				{// if header is there
				 // check the item exist in the cart or not
				 //as cart detail only has one item to add int he cart at a time. with the same cart user
					var cartDetailsDb = await _db.cartDetails.AsNoTracking().FirstOrDefaultAsync(cd =>
					cd.ProductId == cartDto.CartDetails
					.First()
					.ProductId &&
					cd.CartHeaderId == cartHeaderDb.CartHeaderId);

					if (cartDetailsDb == null)
					{
						//create cart details 

						cartDto.CartDetails.First().CartHeaderId = cartHeaderDb.CartHeaderId;
						_db.cartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
						await _db.SaveChangesAsync();

					}
					else
					{
						//update the item count in the cart.

						cartDto.CartDetails.First().Count += cartDetailsDb.Count;
						cartDto.CartDetails.First().CartHeaderId = cartDetailsDb.CartHeaderId;
						cartDto.CartDetails.First().CartDeatailsId = cartDetailsDb.CartDeatailsId;

						_db.cartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
						await _db.SaveChangesAsync();
					}
				}

				_responseDto.Result = cartDto;

			}
			catch (Exception ex)
			{

				_responseDto.Message = ex.Message.ToString();
				_responseDto.IsSuccess = false;
			}





			return _responseDto;

		}


		[HttpPost("RemoveCart")]
		public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
		{
			try
			{
				CartDetails cartDetails = await _db.cartDetails
					.FirstAsync(cd => cd.CartDeatailsId == cartDetailsId);

				int totalCountofCartItem = _db.cartDetails
					.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

				_db.Remove(cartDetails);

				if (totalCountofCartItem == 1)
				{
					var cartHeaderToRemove = await _db.cartHeaders
						.FirstOrDefaultAsync(ch => ch.CartHeaderId == cartDetails.CartHeaderId);

					_db.Remove(cartHeaderToRemove);
				}

				await _db.SaveChangesAsync();


				_responseDto.Result = true;

			}
			catch (Exception ex)
			{

				_responseDto.Message = ex.Message.ToString();
				_responseDto.IsSuccess = false;
			}





			return _responseDto;

		}

		[HttpPost("EmailCartRequest")]
		public async Task<object> EmailCartRequest([FromBody] CartDto cartDto)
		{
			try
			{
				await _messageBus.PublishMessage(_config.GetValue<string>(
					"topicandQueueNames:EmailShoppingCart"), cartDto);

				_responseDto.Result = true;
			}
			catch (Exception ex)
			{

				_responseDto.Message = ex.Message.ToString();
				_responseDto.IsSuccess = false;
			}

			return _responseDto;

		}

	}
}
