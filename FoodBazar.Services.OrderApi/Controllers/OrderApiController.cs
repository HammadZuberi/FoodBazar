using AutoMapper;
using foodBazar.MessageBus;
using FoodBazar.Services.OrderApi.Data;
using FoodBazar.Services.OrderApi.Models;
using FoodBazar.Services.OrderApi.Models.Dto;
using FoodBazar.Services.OrderApi.Service;
using FoodBazar.Services.OrderApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace FoodBazar.Services.OrderApi.Controllers
{
	[Route("api/order")]
	[ApiController]
	[Authorize]
	public class OrderApiController : ControllerBase
	{
		protected ResponseDto _response;
		private IMapper _mapper;
		private readonly AppDbContext _dbContext;
		private IProductService _productService;
		private readonly IConfiguration _configuration;
		private readonly IMessageBus _messageBus;

		public OrderApiController(AppDbContext dbContext, IMapper mapper
			, IProductService productService, IConfiguration configuration, IMessageBus messageBus)
		{
			_dbContext = dbContext;
			_mapper = mapper;
			_productService = productService;
			_response = new ResponseDto();
			_configuration = configuration;
			_messageBus = messageBus;

		}

		[HttpPost("CreateOrder")]
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



		[HttpPost("CreateStripeSession")]
		public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequest)
		{
			try
			{

				var options = new SessionCreateOptions
				{
					//SuccessUrl = "https://example.com/success",
					CancelUrl = stripeRequest.CancelUrl,
					SuccessUrl = stripeRequest.ApprovedUrl,
					//number of products shown in a page checkout
					LineItems = new List<SessionLineItemOptions>(),
					Mode = "payment",
				};
				var Discountsobj = new List<SessionDiscountOptions>()
				{
					new SessionDiscountOptions()
					{
					Coupon = stripeRequest.orderHeader.CouponCode
					}
				};


				//add products in stripe
				foreach (var item in stripeRequest.orderHeader.OrderDetails)
				{
					var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)((item.Price) * 100), //$20.99 ->2099
							Currency = "usd",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Product.Name,
								Description = item.Product.Description,
								//Images =

							}
						},
						Quantity = item.Count
					};
					options.LineItems.Add(sessionLineItem);
				}

				if (stripeRequest.orderHeader.Discount > 0)
				{
					options.Discounts = Discountsobj;
				}

				var service = new SessionService();
				Session session = service.Create(options);

				//stripeRequest.StripeSessionId = session.Id;
				stripeRequest.StripeSessionUrl = session.Url;

				// add to DB to Track payment susscessfull or apply refund options

				OrderHeader orderHeader = await _dbContext.OrderHeader.FirstAsync(u => u.OrderHeaderId == stripeRequest.orderHeader.OrderHeaderId);
				orderHeader.StripeSessionId = session.Id;
				await _dbContext.SaveChangesAsync();

				_response.Result = stripeRequest;
			}
			catch (Exception ex)
			{
				_response.Message = ex.Message.ToString();
				_response.IsSuccess = false;
			}
			return _response;
		}




		[HttpPost("ValidateStripeSession")]
		public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
		{
			try
			{

				OrderHeader orderHeader = await _dbContext.OrderHeader.FirstAsync(u => u.OrderHeaderId == orderHeaderId);
				var service = new SessionService();
				Session session = service.Get(orderHeader.StripeSessionId);

				var paymentIntentservice = new PaymentIntentService();
				PaymentIntent paymentIntent = paymentIntentservice.Get(session.PaymentIntentId);

				if (paymentIntent.Status == "succeeded")
				{
					//if payment is sucessful
					orderHeader.PaymentIntentId = paymentIntent.Id;
					orderHeader.Status = SD.Status_Approved;
					await _dbContext.SaveChangesAsync();

					RewardsDto rewards = new RewardsDto()
					{
						OrderId = orderHeader.OrderHeaderId,
						UserId = orderHeader.UserId,
						//1$ equals to 1 rewards point
						RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal)
					};
					//sending to ime
					string topicname = _configuration.GetValue<string>("topicandQueueNames:OrderCreatedTopic");

					_messageBus.PublishMessage(topicname, rewards);

					_response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
				}

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
