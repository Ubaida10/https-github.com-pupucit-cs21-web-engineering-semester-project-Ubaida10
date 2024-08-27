using CineTix.Models.Entity_Classes;
using CineTix.Models.Interfaces;
using CineTix.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CineTix.Controllers;

public class CinemasController : Controller
{
    private readonly ICinemaRepository _cinemaRepository;
    
    public CinemasController(ICinemaRepository cinemaRepository)
    {
        _cinemaRepository = cinemaRepository;
    }
    // GET
    public IActionResult Index()
    {
        var cinemas = _cinemaRepository.GetAllCinemas();
        return View(cinemas);
    }
    
    public IActionResult Create()
    {
        return View(new Cinema());
    }
    
    public IActionResult Delete()
    {
        return View(new Cinema());
    }
    
    public IActionResult Edit()
    {
        return View(new Cinema());
    }
    
    public IActionResult Details()
    {
        Cinema cinema = new Cinema();
        cinema = _cinemaRepository.GetCinemaById(1);
        return View(cinema);
    }
}