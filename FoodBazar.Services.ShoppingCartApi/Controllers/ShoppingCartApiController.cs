using AutoMapper;
using FoodBazar.Services.ShoppingCartApi.Data;
using FoodBazar.Services.ShoppingCartApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodBazar.Services.ShoppingCartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    //[Authorize]
    public class ShoppingCartApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;
        private ResponseDto _responseDto;

        public ShoppingCartApiController(IMapper apper, AppDbContext dbContext)

        {
            _mapper = apper;
            _db = dbContext;
            _responseDto = new ResponseDto();

        }

        [HttpPost("Cartupsert")]
        public async Task<ResponseDto> Upsert([FromBody] CartDto cartDto)
        {




            return _responseDto;

        }


    }
}
