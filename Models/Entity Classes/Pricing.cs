namespace CineTix.Models.Entity_Classes
{
    public class Pricing
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal Price { get; set; }
        public Movie Movie { get; set; }
    }
}
