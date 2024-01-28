using FoodBazar.Services.Web.Services.IServices;
using FoodBazar.Web.Services;
using FoodBazar.Web.Services.IServices;
using FoodBazar.Web.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();

//second method
builder.Services.AddHttpClient("Coupon", u => u.BaseAddress =
new Uri(builder.Configuration["ServiceUrls:CouponApi"]))
    .AddHttpMessageHandler<ApiAuthenticationHttpClientHandler>();

//add token for both api 
SD.CouponApiUri = builder.Configuration["ServiceUrls:CouponApi"];
SD.AuthApiUri = builder.Configuration["ServiceUrls:AuthApi"];
SD.ProductApiUri = builder.Configuration["ServiceUrls:ProductApi"];
SD.CartApiUri = builder.Configuration["ServiceUrls:CartAPi"];
SD.OrderApiUri = builder.Configuration["ServiceUrls:OrderApi"];

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

//add authentication thorugh cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(10);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>

    {
        endpoints.MapControllerRoute(
        name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
    }
);

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
