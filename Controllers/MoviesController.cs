using CineTix.Data;
using CineTix.Models.Entity_Classes;
using CineTix.Models.Interfaces;
using CineTix.Models.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CineTix.Controllers;

public class MoviesController : Controller
{
    private readonly IMovieRepository _movieRepository;
    private readonly IProducerRepository _producerRepository;
    private readonly IActorRepository _actorRepository;
    private readonly ICinemaRepository _cinemaRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public MoviesController(IMovieRepository movieRepository, IActorRepository actorRepository, ICinemaRepository cinemaRepository, IProducerRepository producerRepository, IWebHostEnvironment webHostEnvironment)
    {
        _movieRepository = movieRepository;
        _actorRepository = actorRepository;
        _cinemaRepository = cinemaRepository;
        _producerRepository = producerRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET
    public IActionResult Index()
    {
        var movies = _movieRepository.GetAll();
        return View(movies);
    }

    // GET: Movies/Create
    [HttpGet]
    public IActionResult Create()
    {
        //var cinemas = _cinemaRepository.GetAllCinemas();
        var producers = _producerRepository.GetAll();
        var actors = _actorRepository.GetAll();
        
        //ViewBag.Cinemas = new SelectList(cinemas, "Id", "Name");
        ViewBag.Producers = new SelectList(producers, "Id", "Name");
        ViewBag.Actors = new MultiSelectList(actors, "Id", "Name");

        return View();
    }

    // POST: Movies/Create
    [HttpPost]
    public IActionResult Create(NewMovie movie, IFormFile MoviePosterUpload, IFormFile MoviePosterPortrait)
    {
        if (MoviePosterUpload?.Length > 0 && MoviePosterPortrait?.Length > 0)
        {
            //Both images of movie has been added.
            movie.Duration = new TimeSpan(movie.Hours, movie.Minutes, 0);

            // Define the path for image 1 where the image will be saved
            string uploadsFolder1 = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Movies");
            string uniqueFileName1 = Guid.NewGuid().ToString() + "_" + MoviePosterUpload.FileName;
            string filePath1 = Path.Combine(uploadsFolder1, uniqueFileName1);


            // Define the path for image 2 where the image will be saved
            string uploadsFolder2 = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Movies");
            string uniqueFileName2 = Guid.NewGuid().ToString() + "_" + MoviePosterPortrait.FileName;
            string filePath2 = Path.Combine(uploadsFolder2, uniqueFileName2);


            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder1))
            {
                Directory.CreateDirectory(uploadsFolder1);
            }

            // Save the image to the server
            using (var fileStream = new FileStream(filePath1, FileMode.Create))
            {
                MoviePosterUpload.CopyTo(fileStream);
            }

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder2))
            {
                Directory.CreateDirectory(uploadsFolder2);
            }

            // Save the image to the server
            using (var fileStream = new FileStream(filePath2, FileMode.Create))
            {
                MoviePosterPortrait.CopyTo(fileStream);
            }

            movie.ImageUrl = "/Images/Movies/" + uniqueFileName1;
            movie.PortraitUrl = "/Images/Movies/" + uniqueFileName2;
            _movieRepository.Add(movie);
            return RedirectToAction("Index");
        }

        //var cinemas = _cinemaRepository.GetAllCinemas();
        var producers = _producerRepository.GetAll();
        var actors = _actorRepository.GetAll();
        
        //ViewBag.Cinemas = new SelectList(cinemas, "Id", "Name");
        ViewBag.Producers = new SelectList(producers, "Id", "Name");
        ViewBag.Actors = new MultiSelectList(actors, "Id", "Name");

        return View(movie);
    }

    public IActionResult Details(int id)
    {
        Movie movie = new Movie();
        //movie.Cinema = new Cinema();
        movie.Producer = new Producer();
        movie = _movieRepository.GetById(id);

        if (movie != null)
        {
            //movie.Cinema = _cinemaRepository.GetCinemaById(movie.CinemaId);
            movie.Producer = _producerRepository.GetById(movie.ProducerId);
            movie.Actor = _actorRepository.GetById(movie.ActorId);
        }
        
        return View(movie);
    }


    public IActionResult Search(string searchString)
    {
        Movie movie = new Movie();
        //movie.Cinema = new Cinema();
        movie.Producer = new Producer();
        
        
        movie = _movieRepository.GetMovieByName(searchString);
        if (movie != null)
        {
            //movie.Cinema = _cinemaRepository.GetCinemaById(movie.CinemaId);
            movie.Producer = _producerRepository.GetById(movie.ProducerId);
            movie.Actor = _actorRepository.GetById(movie.ActorId);
        }
        
        return PartialView("_Details", movie);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var movie = _movieRepository.GetById(id);

        var newMovie = new NewMovie();
        newMovie.Title = movie.Title;
        newMovie.Synopsis = movie.Synopsis;
        newMovie.Duration = movie.Duration;
        newMovie.ReleaseDate = movie.ReleaseDate;
        newMovie.Price = movie.Price;
        newMovie.ImageUrl = movie.ImageUrl;
        newMovie.RottenTomatoScore = movie.RottenTomatoScore;
        newMovie.Genre = movie.Genre;
        newMovie.ProducerId = movie.ProducerId;
        //newMovie.CinemaId = movie.CinemaId;
        newMovie.ActorId = movie.ActorId;
        newMovie.Hours = movie.Duration.Hours;
        newMovie.Minutes = movie.Duration.Minutes;
        if (movie == null)
        {
            return RedirectToAction("MovieNotFound");
        }
        
        //ViewBag.Cinemas = new SelectList(_cinemaRepository.GetAllCinemas(), "Id", "Name", movie.CinemaId);
        ViewBag.Producers = new SelectList(_producerRepository.GetAll(), "Id", "Name", movie.ProducerId);
        ViewBag.Actors = new SelectList(_actorRepository.GetAll(), "Id", "Name", movie.ActorId);

        return View(newMovie);
    }

    [HttpPost]
    public IActionResult Edit(NewMovie movie, IFormFile MoviePosterUpload, IFormFile MoviePosterPortrait)
    {
        if (MoviePosterUpload?.Length > 0 && MoviePosterPortrait?.Length > 0)
        {
            //Both images of movie has been added.
            movie.Duration = new TimeSpan(movie.Hours, movie.Minutes, 0);

            // Define the path for image 1 where the image will be saved
            string uploadsFolder1 = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Movies");
            string uniqueFileName1 = Guid.NewGuid().ToString() + "_" + MoviePosterUpload.FileName;
            string filePath1 = Path.Combine(uploadsFolder1, uniqueFileName1);


            // Define the path for image 2 where the image will be saved
            string uploadsFolder2 = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Movies");
            string uniqueFileName2 = Guid.NewGuid().ToString() + "_" + MoviePosterPortrait.FileName;
            string filePath2 = Path.Combine(uploadsFolder2, uniqueFileName2);


            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder1))
            {
                Directory.CreateDirectory(uploadsFolder1);
            }

            // Save the image to the server
            using (var fileStream = new FileStream(filePath1, FileMode.Create))
            {
                MoviePosterUpload.CopyTo(fileStream);
            }

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder2))
            {
                Directory.CreateDirectory(uploadsFolder2);
            }

            // Save the image to the server
            using (var fileStream = new FileStream(filePath2, FileMode.Create))
            {
                MoviePosterPortrait.CopyTo(fileStream);
            }

            movie.ImageUrl = "/Images/Movies/" + uniqueFileName1;
            movie.PortraitUrl = "/Images/Movies/" + uniqueFileName2;
            _movieRepository.Update(movie);
            return RedirectToAction("Index");
        }
        //var cinemas = _cinemaRepository.GetAllCinemas();
        var producers = _producerRepository.GetAll();
        var actors = _actorRepository.GetAll();

        //ViewBag.Cinemas = new SelectList(cinemas, "Id", "Name");
        ViewBag.Producers = new SelectList(producers, "Id", "Name");
        ViewBag.Actors = new MultiSelectList(actors, "Id", "Name");

        return View(movie);
    }
    public IActionResult Delete(int id)
    {
        _movieRepository.Delete(id);
        return RedirectToAction("Index");
    }

    public IActionResult MovieNotFound()
    {
        return View();
    }
}