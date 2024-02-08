using System.ComponentModel.DataAnnotations;

namespace eKoncert.Models
{
    public class Tickets
    {
        [Key]
        public int Id { get; set; }

        public int EventId { get; set; }

        public string UserId { get; set; }


    }
}
