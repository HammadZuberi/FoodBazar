using AutoMapper;
using FoodBazar.Services.CouponApi.Data;
using FoodBazar.Services.CouponApi.Model;
using FoodBazar.Services.CouponApi.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodBazar.Services.CouponApi.Controllers
{
    [Route("api/[controller]")]
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

        public async Task<ResponseDto> Post([FromBody] CouponDto couponentry)
        {

            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponentry);

                await _appDbContext.Coupons.AddAsync(obj);

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




        [HttpPut]

        public  ResponseDto Put([FromBody] CouponDto couponentry)
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

        public async Task<ResponseDto> Delete(int id)
        {

            try
            {
                Coupon obj = await _appDbContext.Coupons.FirstAsync(u => u.CouponId == id);
                _appDbContext.Coupons.Remove(obj);

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


    }

}
