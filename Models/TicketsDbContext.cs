using Microsoft.EntityFrameworkCore;


namespace eKoncert.Models
{
    public class TicketsDbContext : DbContext
    {

        public DbSet<Tickets> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TicketsDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tickets>().Property(t => t.Id).ValueGeneratedOnAdd();



            base.OnModelCreating(modelBuilder);

        }

    }
}
