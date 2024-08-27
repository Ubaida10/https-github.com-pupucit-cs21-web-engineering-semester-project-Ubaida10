using CineTix.Data;
using CineTix.Models.Entity_Classes;
using CineTix.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CineTix.Controllers;

public class OrdersController: Controller
{
    public IActionResult Index()
    {
        var orders = new List<Order>();
        return View(orders);
    }
    
    public IActionResult OrderCompleted()
    {
        return View();
    }
    
    public IActionResult ShopingCart(int movieId)
    {
        // Create an instance of the repository
        var movieRepository = new MovieRepository();
    
        // Retrieve the movie from the repository
        var movie = movieRepository.GetById(movieId);
    
        // Check if the movie was found
        if (movie == null)
        {
            // Handle the case when the movie is not found
            return NotFound();
        }

        // Create a new cart instance
        Cart cart = new Cart
        {
            CartId = "1", // Generate or retrieve the CartId dynamically
            User = new ApplicationUser { Name = "Abubakar" }, // Assign a real user dynamically
            Items = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    Movie = movie, // Use the existing movie object
                    Amount = 1
                }
            }
        };

        // Create a list of carts and add the cart to it
        var carts = new List<Cart> { cart };

        // Return the view with the carts list
        return View(carts);
    }
    
    public IActionResult RemoveFromCart(int id)
    {
        return View();
    }
}