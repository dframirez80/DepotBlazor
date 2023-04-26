using Domain.Entities;
using Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Repository.EntityDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductMovement> ProductMovements { get; set; }
        public DbSet<Depot> Depots { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1, Created = DateTime.Now, Modified = DateTime.Now, Name = "user1@test", Password = TripleDES.Encrypt("Test_1234")
                },
                new User
                {
                    Id = 2,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Name = "user2@test",
                    Password = TripleDES.Encrypt("Test_1234")
                });
        }
    }
}