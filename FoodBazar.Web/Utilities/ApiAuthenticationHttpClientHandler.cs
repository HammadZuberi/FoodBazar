
using Microsoft.AspNetCore.Authentication;

namespace FoodBazar.Web.Utilities
{
	public class ApiAuthenticationHttpClientHandler : DelegatingHandler
	{
		private readonly IHttpContextAccessor _contextAccessor;
		public ApiAuthenticationHttpClientHandler(IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			//where the token is stored
			var token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
			return await base.SendAsync(request, cancellationToken);
		}
	}
}
