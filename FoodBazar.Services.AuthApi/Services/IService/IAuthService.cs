using FoodBazar.Services.AuthApi.Models.Dto;

namespace FoodBazar.Services.AuthApi.Services.IService
{
	public interface IAuthService
	{
		public Task<LoginResponseDto>  Login(LoginRequestDto loginRequest);
		public Task<string> Register(RegistrationRequestDto registerRequestDto);

		public Task<bool> AssignRole(string email, string roleName);
	}
}
