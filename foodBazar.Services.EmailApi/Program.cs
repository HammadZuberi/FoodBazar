using Microsoft.EntityFrameworkCore;
using FoodBazar.Services.EmailApi.Data;
using foodBazar.Services.EmailApi.Messaging;
using foodBazar.Services.EmailApi.Extension;
using foodBazar.Services.EmailApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string SQLConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(SQLConnection);

});

var optionBuilder= new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseSqlServer(SQLConnection);
builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

builder.Services.AddControllers();
builder.Services.AddSingleton
	<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

DBMigrations();
//up and running when app starts
app.UseAzureServiceBusCoinsumer();
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