using CineTix.Models.Entity_Classes;
using CineTix.Models.Interfaces;
using CineTix.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CineTix.Controllers;

public class ProducersController : Controller
{
    private readonly IProducerRepository _producerRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProducersController(IProducerRepository producerRepository, IWebHostEnvironment webHostEnvironment)
    {
        _producerRepository = producerRepository;
        _webHostEnvironment = webHostEnvironment;
    }
    // GET
    public IActionResult Index()
    {
        var producers = _producerRepository.GetAll();
        return View(producers);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Producer());
    }

    [HttpPost]
    public IActionResult Create(Producer producer, IFormFile ProfilePictureUpload)
    {

        if (ProfilePictureUpload?.Length>0)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Producers");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePictureUpload.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                ProfilePictureUpload.CopyTo(fileStream);
            }

            producer.ProfilePictureUrl = "/Images/Producers/" + uniqueFileName;
            _producerRepository.Add(producer);
            return RedirectToAction(nameof(Index));
        }

        
        return View(producer);
        
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producer = _producerRepository.GetById(id);
        if (producer== null)
        {
            return RedirectToAction("ProducerNotFound");
        }
        return View(producer);
    }

    [HttpPost]
    public IActionResult Edit(Producer producer, IFormFile ProfilePictureUpload)
    {
        if (ProfilePictureUpload != null && ProfilePictureUpload.Length > 0)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Producers");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePictureUpload.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                ProfilePictureUpload.CopyTo(fileStream);
            }

            producer.ProfilePictureUrl = "/Images/Producers/" + uniqueFileName;
            _producerRepository.Update(producer);
            return RedirectToAction("Index");
        }
        else
        {
            return View(producer);
        }
    }

    public IActionResult Details(int id)
    {
        Producer producer = new Producer();
        producer = _producerRepository.GetById(id);
        return View(producer);
    }
    
    public IActionResult Delete(int id)
    {
        _producerRepository.Delete(id);
        return RedirectToAction("Index");
    }
}