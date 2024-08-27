using System.ComponentModel.DataAnnotations;
using CineTix.Models.Entity_Classes;

namespace CineTix.Data
{
    public class Cart
    {
        [Key]
        public string CartId { get; set; }

        public ApplicationUser User { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal Total => Items.Sum(item => item.Movie.Price * item.Amount);
    }

}