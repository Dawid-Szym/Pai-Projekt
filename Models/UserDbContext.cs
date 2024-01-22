using Microsoft.EntityFrameworkCore;

namespace eKoncert.Models
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("UserDb");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "user1", Password = "password1", AccountType = "logged user" },
                new User { Id = 2, Username = "admin1", Password = "password2", AccountType = "admin" },
                new User { Id = 3, Username = "manager1", Password = "password3", AccountType = "event manager" }
            );



            base.OnModelCreating(modelBuilder);
        }






    }
}
