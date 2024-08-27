using System.ComponentModel.DataAnnotations;

namespace CineTix.Models.Entity_Classes
{
    public class ApplicationUser
    {
        [Display(Name = "Full name")]
        public string Name { get; set; }
    }
}
