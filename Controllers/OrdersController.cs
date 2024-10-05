using CineTix.Data;
using CineTix.Models.Entity_Classes;
using CineTix.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        var movieRepository = new MovieRepository();
        var movie = movieRepository.GetById(movieId);

        if (movie == null)
        {
            return NotFound();
        }

        var cart = GetCartFromSession();

        // Check if the movie is already in the cart
        var existingItem = cart.Items.FirstOrDefault(item => item.Movie.Id == movie.Id);
        if (existingItem != null)
        {
            // If it exists, increment the amount
            existingItem.Amount++;
        }
        else
        {
            // Otherwise, add a new item
            cart.Items.Add(new ShoppingCartItem
            {
                Movie = movie,
                Amount = 1
            });
        }

        // Save the updated cart to the session
        SaveCartToSession(cart);

        return View(cart);
    }
    
    [HttpPost]
    public IActionResult RemoveFromCart(int movieId)
    {
        var cart = GetCartFromSession();

        // Check if the cart is empty
        if (cart.Items == null || !cart.Items.Any())
        {
            return Json(cart); // Return the empty cart
        }

        var itemToRemove = cart.Items.FirstOrDefault(item => item.Movie.Id == movieId);
    
        if (itemToRemove != null)
        {
            if (itemToRemove.Amount > 1)
            {
                // Decrease the amount instead of removing the item
                itemToRemove.Amount--;
            }
            else
            {
                // Remove the item if the amount is 1
                cart.Items.Remove(itemToRemove);
            }
        
            SaveCartToSession(cart);
        }

        // Return the updated cart as JSON
        return Json(cart);
    }


    
    public IActionResult UpdateCart(int movieId, int amount)
    {
        var cart = GetCartFromSession();
        var existingItem = cart.Items.FirstOrDefault(item => item.Movie.Id == movieId);

        if (existingItem != null)
        {
            // Update the amount if it's greater than zero
            if (amount <= 0)
            {
                cart.Items.Remove(existingItem);
            }
            else
            {
                existingItem.Amount = amount;
            }
        
            SaveCartToSession(cart);
        }

        return RedirectToAction("ShopingCart");
    }

    private Cart GetCartFromSession()
    {
        var existingCarts = HttpContext.Session.GetString("Cart");
        return existingCarts != null ? JsonConvert.DeserializeObject<Cart>(existingCarts) : new Cart { Items = new List<ShoppingCartItem>() };
    }

    private void SaveCartToSession(Cart cart)
    {
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }
}