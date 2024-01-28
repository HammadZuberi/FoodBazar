using foodBazar.MessageBus;
using FoodBazar.Services.OrderApi.Data;
using FoodBazar.Services.OrderApi.Extensions;
using FoodBazar.Services.OrderApi.Service;
using FoodBazar.Services.OrderApi.Utilities;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddEndpointsApiExplorer();
builder.AddSwaggerwithAuth();

builder.Services.AddScoped<IProductService, FoodBazar.Services.OrderApi.Service.ProductService>();
builder.Services.AddScoped<IMessageBus,MessageBus>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ApiAuthenticationHttpClientHandler>();

builder.Services.AddHttpClient("Product", u=> u.BaseAddress = 
new Uri(builder.Configuration["ServiceUrl:ProductAPi"]))
	.AddHttpMessageHandler<ApiAuthenticationHttpClientHandler>();

builder.Services.AddControllers();
//ext aut and auth
builder.AddAppAuthentication();
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.AddMigrations();
}

StripeConfiguration.ApiKey =builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
