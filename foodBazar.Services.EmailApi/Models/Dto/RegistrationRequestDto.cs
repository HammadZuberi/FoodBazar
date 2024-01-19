using System.ComponentModel.DataAnnotations;

namespace foodBazar.Services.EmailApi.Models.Dto
{
	public class RegistrationRequestDto
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string? Role { get; set; }

	}
}
