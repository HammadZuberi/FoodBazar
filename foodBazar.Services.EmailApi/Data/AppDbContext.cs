
using foodBazar.Services.EmailApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodBazar.Services.EmailApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<EmailLogger> EmailLoggers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }
    }
}
