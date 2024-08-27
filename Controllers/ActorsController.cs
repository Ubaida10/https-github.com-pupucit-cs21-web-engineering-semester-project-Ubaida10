using CineTix.Models.Entity_Classes;
using CineTix.Models.Interfaces;
using CineTix.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CineTix.Controllers;

public class ActorsController : Controller
{
    private readonly IActorRepository _actorRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ActorsController(IActorRepository actorRepository, IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        _actorRepository = actorRepository;
    }
    // GET
    public IActionResult Index()         //Default ActionResult
    {
        var acts = _actorRepository.GetAll();
        return View(acts);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Actor());
    }

    [HttpPost]
    public IActionResult Create(Actor actor, IFormFile ProfilePictureUpload)
    {
        if (ProfilePictureUpload?.Length > 0)
        {
            // Define the path where the image will be saved
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Actors");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePictureUpload.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Save the image to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                ProfilePictureUpload.CopyTo(fileStream);
            }

            // Set the ProfilePictureUrl property after saving the file
            actor.ProfilePictureUrl = "/Images/Actors/" + uniqueFileName;
            Console.WriteLine(actor.ProfilePictureUrl);
            // Clear the model state for the ProfilePictureUrl field
            _actorRepository.Add(actor);
            return RedirectToAction(nameof(Index));
        }

        // If we got this far, something failed, redisplay form
        return View(actor);
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        var actor = _actorRepository.GetById(id);
        if (actor == null)
        {
            return RedirectToAction("ActorNotFound");
        }
        return View(actor);
    }

    [HttpPost]
    public IActionResult Edit(Actor actor, IFormFile ProfilePictureUpload)
    {
        ModelState.Clear();
        if (ProfilePictureUpload?.Length > 0)
        {
            // Define the path where the image will be saved
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Actors");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePictureUpload.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Save the image to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                ProfilePictureUpload.CopyTo(fileStream);
            }

            // Set the ProfilePictureUrl property after saving the file
            actor.ProfilePictureUrl = "/Images/Actors/" + uniqueFileName;
            Console.WriteLine(actor.ProfilePictureUrl);
            
            _actorRepository.Update(actor);
            return RedirectToAction(nameof(Index));
        }

        // If we got this far, something failed, redisplay form
        return View(actor);
    }

    public IActionResult Details(int id)
    {
        Actor actor = new Actor();
        actor = _actorRepository.GetById(id);
        return View(actor);
    }
    
    public IActionResult Delete(int id)
    {
        _actorRepository.Delete(id);
        return RedirectToAction("Index");
    }
    
    public IActionResult ActorNotFound()
    {
        return View();
    }
}