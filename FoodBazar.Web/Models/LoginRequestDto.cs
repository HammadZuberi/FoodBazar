using System.ComponentModel.DataAnnotations;

namespace FoodBazar.Services.Web.Models.Dto
{
	public class LoginRequestDto
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
    }
}
