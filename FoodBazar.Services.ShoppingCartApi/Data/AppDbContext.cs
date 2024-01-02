using FoodBazar.Services.ShoppingCartApi.Model;
using FoodBazar.Services.ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodBazar.Services.ShoppingCartApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<CartHeader> cartHeaders { get; set; }
        public DbSet<CartDetails> cartDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         
        }
    }
}
