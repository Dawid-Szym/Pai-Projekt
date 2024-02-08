using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int? CreatedBy { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public byte[]? ImageData { get; set; }

        public string? ImageMimeType { get; set; }
    }
}
