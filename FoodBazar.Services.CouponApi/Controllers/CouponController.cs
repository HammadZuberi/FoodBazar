﻿using AutoMapper;
using FoodBazar.Services.CouponApi.Data;
using FoodBazar.Services.CouponApi.Model;
using FoodBazar.Services.CouponApi.Model.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//using Stripe;
//using Coupon = FoodBazar.Services.CouponApi.Model.Coupon;

namespace FoodBazar.Services.CouponApi.Controllers
{
	[Route("api/coupon")]
	[Authorize]
	[ApiController]
	public class CouponController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;
		private ResponseDto _response;
		private IMapper _mapper;
		public CouponController(AppDbContext appDbContext, IMapper mapper)
		{
			this._appDbContext = appDbContext;
			this._mapper = mapper;
			_response = new ResponseDto();

		}

		[HttpGet]
		public async Task<ResponseDto> Get()
		{

			try
			{

				IEnumerable<Coupon> objList = await _appDbContext.Coupons.ToListAsync();
				_response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);

			}
			catch (Exception ex)
			{
				_response.Message = ex.Message;
				_response.IsSuccess = false;


			}
			return _response;
		}


		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResponseDto> Get(int id)
		{

			try
			{

				Coupon obj = await _appDbContext.Coupons.FirstOrDefaultAsync(u => u.CouponId == id);
				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{

				_response.Message = ex.Message;
				_response.IsSuccess = false;

			}
			return _response;
		}


		[HttpGet]
		[Route("GetCode/{code}")]
		[AllowAnonymous]
		public async Task<ResponseDto> GetCode(string code)
		{

			try
			{

				Coupon obj = await _appDbContext.Coupons.FirstOrDefaultAsync(u => u.CouponCode.ToLower() == code.ToLower());
				if (obj == null)
				{
					_response.Message = "Coupon not found";

				}
				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{

				_response.Message = ex.Message;
				_response.IsSuccess = false;

			}
			return _response;
		}



		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		public async Task<ResponseDto> Post([FromBody] CouponDto couponentry)
		{

			try
			{
				Coupon couponDto = _mapper.Map<Coupon>(couponentry);

				await _appDbContext.Coupons.AddAsync(couponDto);

				_appDbContext.SaveChanges();



				var options = new Stripe.CouponCreateOptions
				{
					//Duration = "repeating",
					//DurationInMonths = 3,
					//PercentOff
					Name= couponDto.CouponCode,
					Id=couponDto.CouponCode,
					Currency="usd",
					AmountOff = (long)(couponDto.DiscountAmount * 100),
				};

				var service = new Stripe.CouponService();
				service.Create(options);

				_response.Result = _mapper.Map<CouponDto>(couponDto);
			}
			catch (Exception ex)
			{

				_response.Message = ex.Message;
				_response.IsSuccess = false;

			}
			return _response;
		}




		[HttpPut]
		[Authorize(Roles = "ADMIN")]
		public ResponseDto Put([FromBody] CouponDto couponentry)
		{

			try
			{
				Coupon obj = _mapper.Map<Coupon>(couponentry);

				_appDbContext.Coupons.Update(obj);

				_appDbContext.SaveChanges();
				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{

				_response.Message = ex.Message;
				_response.IsSuccess = false;

			}
			return _response;
		}


		[HttpDelete]
		[Route("{id:int}")]
		[Authorize(Roles = "ADMIN")]
		public async Task<ResponseDto> Delete(int id)
		{

			try
			{
			
				Coupon obj = await _appDbContext.Coupons.FirstAsync(u => u.CouponId == id);
				_appDbContext.Coupons.Remove(obj);

				_appDbContext.SaveChanges();


				var service = new Stripe.CouponService();
				service.Delete(obj.CouponCode);

				_response.Result = _mapper.Map<CouponDto>(obj);
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
