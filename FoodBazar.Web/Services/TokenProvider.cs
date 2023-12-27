using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;
using Newtonsoft.Json.Linq;

namespace FoodBazar.Web.Services
{
	public class TokenProvider : ITokenProvider
	{
		private readonly IHttpContextAccessor _contextAccessor;

		public TokenProvider(IHttpContextAccessor httpContextAccessor)
		{
			_contextAccessor = httpContextAccessor;

		}
		public void ClearToken()
		{
			_contextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);
		}

		public string GetToken()
		{

			string? token = null;

			bool? isTrue = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out token);

			return isTrue is true ? token : null;

		}

		public void SetToken(string token)
		{
			_contextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
		}
	}
}
