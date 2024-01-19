using FoodBazar.Services.Web.Models.Dto;
using FoodBazar.Web.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace FoodBazar.Web.Services.IServices
{
	public interface IAuthService
	{
		public Task<ResponseDto?> LoginAsync(LoginRequestDto requestDto);	 
		public Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto requestDto);	 
		public Task<ResponseDto?> RegisterAsync(RegistrationRequestDto requestDto);	 
		//public Task<ResponseDto?> EmailUserRegister(RegistrationRequestDto requestDto);	 
	}
}
