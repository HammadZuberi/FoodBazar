using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace FoodBazar.Services.OrderApi.Extensions
{
	public static class EndPointExtensions
	{


		public static WebApplicationBuilder AddSwaggerwithAuth(this WebApplicationBuilder builder)
		{
			//Auto Mapper Config
			IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
			builder.Services.AddSingleton(mapper);
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			builder.Services.AddSwaggerGen(opt =>
			{
				opt.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Description = "Enter the Bearer token string as follows :` Bearer JWT-Token",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				opt.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference= new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id=JwtBearerDefaults.AuthenticationScheme
				}
			},new string []{}
		}
	});
			});

			return builder;
		}
	}
}
