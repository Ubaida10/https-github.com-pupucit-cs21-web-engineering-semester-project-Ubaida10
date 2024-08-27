using CineTix.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CineTix.Models.Entity_Classes
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string PortraitUrl { get; set; }
        public double RottenTomatoScore { get; set; }
        public Genre Genre { get; set; }

        public List<MovieActor> MovieActors { get; set; }

        //Cinema
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }

        //Producer
        public int ProducerId { get; set; }
        [ForeignKey("ProducerId")]
        public Producer Producer { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
