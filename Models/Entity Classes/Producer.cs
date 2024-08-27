using System.ComponentModel.DataAnnotations;

namespace CineTix.Models.Entity_Classes
{
    public class Producer
    {
        public int Id { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
    }
}
