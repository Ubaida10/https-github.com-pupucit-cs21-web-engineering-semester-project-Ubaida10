using System.ComponentModel.DataAnnotations;

namespace CineTix.Models.Entity_Classes
{
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        public Movie Movie { get; set; }  // Make sure Movie has ImageUrl
        public int Amount { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
