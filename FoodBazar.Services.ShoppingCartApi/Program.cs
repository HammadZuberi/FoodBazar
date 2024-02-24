using AutoMapper;
using FoodBazar.Services.ShoppingCartApi.Extensions;
using FoodBazar.Services.ShoppingCartApi;
using FoodBazar.Services.ShoppingCartApi.Data;
using FoodBazar.Services.ShoppingCartApi.Extensions;
using Microsoft.EntityFrameworkCore;
using FoodBazar.Services.ShoppingCartApi.Service;
using foodBazar.MessageBus;
using FoodBazar.RabbitMQSender;
using FoodBazar.Services.AuthApi.RabittMQSender;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.AddSwaggerwithAuth();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
//builder.Services.AddScoped<IMessageBus, MessageBus>();
builder.Services.AddScoped<IRabbitMQMessageSender, RabbitMQMessageSender>();

builder.Services.AddHttpClient("Product", u => u.BaseAddress =
new Uri(builder.Configuration["ServiceUrl:ProductAPi"]));

builder.Services.AddHttpClient("Coupon", u => u.BaseAddress =
new Uri(builder.Configuration["ServiceUrl:CouponAPi"]));

builder.AddAppAuthentication();
builder.Services.AddAuthentication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	//external migration
	app.AddMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
