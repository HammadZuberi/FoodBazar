using Microsoft.AspNetCore.Identity;

namespace FoodBazar.Services.AuthApi.Models
{
	public class ApplicationUser :IdentityUser
	{
        public int Name { get; set; }
    }

}
