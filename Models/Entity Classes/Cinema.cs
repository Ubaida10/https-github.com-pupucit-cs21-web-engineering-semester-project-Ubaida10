using System.ComponentModel.DataAnnotations;

namespace CineTix.Models.Entity_Classes
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }
        public string CinemaLogoUrl { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ContactInfo { get; set; }
    }
}
