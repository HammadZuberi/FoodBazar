using Microsoft.AspNetCore.Identity;

namespace FoodBazar.Services.AuthApi.Models
{
	public class ApplicationUser :IdentityUser
	{
        public string Name { get; set; }
    }

}
