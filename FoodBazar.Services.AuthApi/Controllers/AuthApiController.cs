using FoodBazar.Services.AuthApi.Models.Dto;
using FoodBazar.Services.AuthApi.Services.IService;
using FoodBazar.Services.CouponApi.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FoodBazar.Services.AuthApi.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthApiController : ControllerBase
	{
		public readonly IAuthService _authService;
		protected ResponseDto _responseDto;
		public AuthApiController(IAuthService authService)
		{
			this._authService = authService;
			_responseDto = new();

		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registration)
		{
			var message = await _authService.Register(registration);
			if (!string.IsNullOrEmpty(message))
			{
				_responseDto.Message = message;
				_responseDto.IsSuccess = false;
				return BadRequest(_responseDto);

			}
			return Ok(_responseDto);
		}


		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
		{
			var loginResponse = await _authService.Login(loginRequest);

			if (loginResponse == null || loginResponse.User == null)
			{
				_responseDto.Message = "The UserName and Password is Incorrect";
				_responseDto.IsSuccess = false;
				return BadRequest(_responseDto);

			}
			_responseDto.Result = loginResponse;
			return Ok(_responseDto);

		}


		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPassword([FromBody] LoginRequestDto loginRequest)
		{
			var loginResponse = await _authService.ResetPassword(loginRequest.UserName,loginRequest.Password);
			
			if (loginResponse == false)
			{
				_responseDto.Message = "The UserName and Password can not reset";
				_responseDto.IsSuccess = false;
				return BadRequest(_responseDto);

			}
			_responseDto.Result = loginResponse;
			return Ok(_responseDto);

		}




		[HttpPost("AssignRole")]
		public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto request)
		{
			var loginResponse = await _authService.AssignRole(request.Email, request.Role.ToUpper());

			if (!loginResponse)
			{
				_responseDto.Message = "Error assigning role";
				_responseDto.IsSuccess = false;
				return BadRequest(_responseDto);
			}

			return Ok(_responseDto);

		}




		
	}
}
