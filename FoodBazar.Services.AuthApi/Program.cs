using foodBazar.MessageBus;
using FoodBazar.Services.AuthApi.Data;
using FoodBazar.Services.AuthApi.Models;
using FoodBazar.Services.AuthApi.RabbitMQSender;
using FoodBazar.Services.AuthApi.RabittMQSender;
using FoodBazar.Services.AuthApi.Services;
using FoodBazar.Services.AuthApi.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});


//default identity setup  configure identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IMessageBus, MessageBus>();
builder.Services.AddScoped<IRabbitMQAuthMessageSender,RabbitMQAuthMessageSender>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth APi");
		c.RoutePrefix = string.Empty;
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DBMigrations();
app.Run();



void DBMigrations()
{
	using (var scope = app.Services.CreateScope())
	{
		var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		if (_db.Database.GetPendingMigrations().Count() > 0)
			_db.Database.Migrate();
	}
}
