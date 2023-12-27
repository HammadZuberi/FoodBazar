using FoodBazar.Services.AuthApi;
using FoodBazar.Services.AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodBazar.Services.AuthApi.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{

		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}




		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

		}
	}
}
