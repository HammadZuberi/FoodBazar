using FoodBazar.Services.AuthApi.Models;

namespace FoodBazar.Services.AuthApi.Services.IService
{
	public interface IJwtTokenGenerator
	{
		 string GenerateToken(ApplicationUser user, IEnumerable<string>? roles);
	}
}
