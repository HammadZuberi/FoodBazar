using FoodBazar.Services.Web.Models.Dto;
using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;

namespace FoodBazar.Web.Services
{
	public class AuthService : IAuthService
	{

		private readonly IBaseService _baseService;
		public AuthService(IBaseService baseService)
		{

			_baseService = baseService;

		}
		public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto requestDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data=requestDto,
				Url = SD.AuthApiUri + "/api/auth/AssignRole"

			});
		}

		public async Task<ResponseDto?> LoginAsync(LoginRequestDto requestDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = requestDto,
				Url = SD.AuthApiUri + "/api/auth/login"

			},withBeaer:false);
		}

		public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto requestDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = requestDto,
				Url = SD.AuthApiUri + "/api/auth/register"

			}, withBeaer: false);
		}
	}
}
