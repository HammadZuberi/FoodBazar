using AutoMapper;
using FoodBazar.Services.Product.Data;
using FoodBazar.Services.Product.Models;
using FoodBazar.Services.Product.Models.Dto;
using FoodBazar.Services.Product.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodBazar.Services.Product.Controllers
{
    [Route("api/product")]
    //[Authorize]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private ResponseDto _response;
        private IProductService _service;

        public ProductApiController(IProductService service)
        {
            _response = new ResponseDto();
            _service = service;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<ResponseDto> Get()
        {

            _response = await _service.GetAllProducts();
            return _response;

        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDto> Get(int id)
        {

            _response = await _service.GetProduct(id);
            return _response;

        }

        [HttpGet]
        [Route("GetName/{Name}")]
        public async Task<ResponseDto> GetByName(string Name)
        {

            _response = await _service.GetProductByName(Name);
            return _response;

        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Put( ProductDto model)
        {

            _response = await _service.UpdateProduct(model);
            return _response;

        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Post( ProductDto model)
        {
            //baseurl htts://localhost:443 etc
            model.ImageLocalPath = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
            _response = await _service.CreateProduct(model);
            return _response;

        }
        [HttpDelete("{id}")]
        //[Route("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Delete(int id)
        {
            _response = await _service.DeleteProduct(id);
            return _response;

        }
    }
}
