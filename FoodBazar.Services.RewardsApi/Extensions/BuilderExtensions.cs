
using FoodBazar.Services.RewardsApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FoodBazar.Services.RewardsApi.Extensions
{
	public static class BuilderExtensions
	{

		public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
		{

			//or
			//var settings = builder.Configuration.GetSection("ApiSettings");
			var secret = builder.Configuration.GetValue<string>("ApiSettings:secret");
			var Issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
			var Audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");

			var key = Encoding.UTF8.GetBytes(secret);
			builder.Services.AddAuthentication(
				x =>
				{
					x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				}
				).AddJwtBearer(y =>
				{
					y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(key),
						ValidateIssuer = true,
						ValidIssuer = Issuer,
						ValidAudience = Audience,
						ValidateAudience = true
					};
				});

			return builder;
		}

		public static void AddMigrations(this IApplicationBuilder app)
		{
			//custom service scope 

			using IServiceScope scope = app.ApplicationServices.CreateScope();

			using AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			if (appDbContext.Database.GetPendingMigrations().Count() > 0)
				appDbContext.Database.Migrate();
		}
	}
}
