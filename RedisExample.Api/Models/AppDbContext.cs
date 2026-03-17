using Microsoft.EntityFrameworkCore;

namespace RedisExample.Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>().HasData(
                new Products { Id = 1, Name = "Product 1", Price = 10.0m },
                new Products { Id = 2, Name = "Product 2", Price = 20.0m },
                new Products { Id = 3, Name = "Product 3", Price = 30.0m }
            );
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Products> Products { get; set; }

    }
}
