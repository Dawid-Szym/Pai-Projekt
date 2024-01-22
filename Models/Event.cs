using System.ComponentModel.DataAnnotations;

namespace eKoncert.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }

        public int? TicketsRemaining { get; set; }

        public string? Description { get; set; }

        public string? Url { get; set; }
    }
}
