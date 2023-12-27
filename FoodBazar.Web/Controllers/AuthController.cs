using FoodBazar.Services.Web.Models.Dto;
using FoodBazar.Web.Models;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FoodBazar.Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly ITokenProvider _tokenProvider;
		public AuthController(IAuthService authService, ITokenProvider tokenProvider)
		{
			this._authService = authService;
			this._tokenProvider = tokenProvider;
		}

		[HttpGet]
		public IActionResult Login()
		{
			LoginRequestDto loginRequestDto = new();
			return View();
		}

		[HttpGet]
		public IActionResult Register()
		{
			var roleList = new List<SelectListItem>()
			{
				new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
				new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer}
			};

			ViewBag.RoleList = roleList;
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Loginout()
		{
			await HttpContext.SignOutAsync();
			_tokenProvider.ClearToken();
			return RedirectToAction("Index", "Home");
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginRequestDto model)
		{

			ResponseDto response = await _authService.LoginAsync(model);


			if (response != null && response.IsSuccess)
			{
				LoginResponseDto loginResponse =
					JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

				await SignInUser(loginResponse);
				_tokenProvider.SetToken(loginResponse.Token);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("CustomError", response.Message);


					TempData["error"] = response.Message.ToString();

				
				return View(model);
			}


		}

		[HttpPost]
		public async Task<IActionResult> Register(RegistrationRequestDto model)
		{
			ResponseDto result = await _authService.RegisterAsync(model);
			ResponseDto assignRole;

			if (result != null && result.IsSuccess)
			{
				if (string.IsNullOrEmpty(model.Role))
				{
					model.Role = SD.RoleCustomer;
				}
				assignRole = await _authService.AssignRoleAsync(model);
				if (assignRole != null && assignRole.IsSuccess)
				{
					TempData["success"] = "Registration Successful";
					return RedirectToAction(nameof(Login));
				}
				else
				{
					TempData["error"] = assignRole.Message.ToString();

				}

			}
			else
			{
				TempData["error"] = result.Message.ToString();

			}

			return RedirectToAction(nameof(Register));

		}

		private async Task SignInUser(LoginResponseDto responseDto)
		{

			var handler = new JwtSecurityTokenHandler();
			var jwt = handler.ReadJwtToken(responseDto.Token);

			var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
			//jwt claims
			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
				jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
				jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
				jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

			// for identityClaims

			identity.AddClaim(new Claim(ClaimTypes.Name,
				jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

			//automatically detect Authorized (Role) in other controllers
			identity.AddClaim(new Claim(ClaimTypes.Role,
				jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

			var principals = new ClaimsPrincipal(identity);

			//signin the user using builtin .net identity  trough claims 
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principals);

		}

	}
}
