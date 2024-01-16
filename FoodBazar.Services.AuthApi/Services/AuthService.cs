using AutoMapper;
using FoodBazar.Services.AuthApi.Data;
using FoodBazar.Services.AuthApi.Models;
using FoodBazar.Services.AuthApi.Models.Dto;
using FoodBazar.Services.AuthApi.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodBazar.Services.AuthApi.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _appDb;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManger;
		private readonly IJwtTokenGenerator _jwtTokenGenerator;

		public AuthService(AppDbContext appDb,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManger,
			IJwtTokenGenerator jwtTokenGenerator)
		{

			this._appDb = appDb;
			_userManager = userManager;
			_roleManger = roleManger;
			_jwtTokenGenerator = jwtTokenGenerator;
		}

		public async Task<bool> AssignRole(string email, string roleName)
		{
			var user = _appDb.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
			if (user != null)
			{
				if (!_roleManger.RoleExistsAsync(roleName).GetAwaiter().GetResult())
				{
					//create role
					_roleManger.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

				}
				await _userManager.AddToRoleAsync(user, roleName);
				return true;
			}
			return false;

		}

		public async Task<bool> ResetPassword(string email, string newPassword)
		{

			//var user = await UserManager.FindByIdAsync(id);

			//var token = await UserManager.GeneratePasswordResetTokenAsync(user);

			//var result = await UserManager.ResetPasswordAsync(user, token, "MyN3wP@ssw0rd");
			var user = _appDb.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
			if (user != null)
			{

				var token = await _userManager.GeneratePasswordResetTokenAsync(user);

				var result = await _userManager.ResetPasswordAsync(user, token, newPassword);


				if (result.Succeeded)
				{
					return true;
				}
				return false;
			}
			return false;

		}



		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
		{
			var user = _appDb.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());

			bool IsValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
			if (user == null || IsValid == false)
			{
				return new LoginResponseDto()
				{
					User = null,
					Token = ""
				};

			}
			// if found generate token
			//read roles
			var roles = await _userManager.GetRolesAsync(user);
			var token = _jwtTokenGenerator.GenerateToken(user, roles);
			UserDto userDto = new UserDto()
			{

				Email = user.Email,
				Id = user.Id,
				Name = user.Name,
				PhoneNumber = user.PhoneNumber
			};

			return new LoginResponseDto()
			{
				User = userDto,
				Token = token
			};


		}

		public async Task<string> Register(RegistrationRequestDto registerRequestDto)
		{
			ApplicationUser user = new ApplicationUser()
			{
				Email = registerRequestDto.Email,
				UserName = registerRequestDto.Email,
				Name = registerRequestDto.Name,
				NormalizedEmail = registerRequestDto.Email.ToUpper(),
				PhoneNumber = registerRequestDto.PhoneNumber,

			};

			try
			{

				var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

				if (result.Succeeded)
				{
					var UserToReturn = await _appDb.ApplicationUsers.FirstAsync(u => u.UserName == registerRequestDto.Email);
					UserDto userDto = new UserDto()
					{

						Email = UserToReturn.Email,
						Id = UserToReturn.Id,
						Name = UserToReturn.Name,
						PhoneNumber = UserToReturn.PhoneNumber
					};

					return "";
					// userDto.ToString();
				}
				else
				{
					return result.Errors.FirstOrDefault().Description;
				}
			}
			catch (Exception ex)
			{

				throw;
			}

			return "";
		}
	}
}
