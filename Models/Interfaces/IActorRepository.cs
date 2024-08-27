using System.Collections;
using CineTix.Models.Entity_Classes;

namespace CineTix.Models.Interfaces;

public interface IActorRepository
{
    //Create
    void Add(Actor actor);

    //Read
    Actor GetById(int id);
    IEnumerable<Actor> GetAll();
    IEnumerable<Movie>GetMoviesByActor(int id);
    
    //Update
    void Update(Actor actor);
    
    //Delete
    void Delete(int id);
}