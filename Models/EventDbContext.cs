using Microsoft.EntityFrameworkCore;

namespace eKoncert.Models
{
    public class EventDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("EventDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            
                modelBuilder.Entity<Event>().HasData(
                    new Event { Id = 1, Name = "Koncert1", DateStart = null, DateEnd = null, TicketsRemaining = 100, Description = "Opis opis opis", Url = "images/concerts/concert1.jpg", CreatedBy= 0 },
                    new Event { Id = 2, Name = "Koncert2", DateStart = null, DateEnd = null, TicketsRemaining = 0, Description = "Opis opis opis", Url = "images/concerts/concert2.jpg", CreatedBy = 0 },
                    new Event { Id = 3, Name = "Koncert3", DateStart = null, DateEnd = null, TicketsRemaining = 100, Description = "Opis opis opis", Url = "images/concerts/concert3.jpg", CreatedBy = 0 },
                    new Event { Id = 4, Name = "Koncert4", DateStart = null, DateEnd = null, TicketsRemaining = 100, Description = "Opis opis opis", Url = "images/concerts/concert4.jpg", CreatedBy = 0 },
                    new Event { Id = 5, Name = "Koncert5", DateStart = null, DateEnd = null, TicketsRemaining = 100, Description = "Opis opis opis", Url = "images/concerts/concert5.jpg", CreatedBy = 0 }
                );
            

            base.OnModelCreating(modelBuilder);
        }


    }
}
