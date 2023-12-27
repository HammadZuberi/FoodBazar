namespace FoodBazar.Services.AuthApi.Models
{
	public class JwtOptions
	{
        public string Audience { get; set; } = string.Empty;
        public string secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
    }
}
