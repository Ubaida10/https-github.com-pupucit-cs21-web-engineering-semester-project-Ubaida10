using CineTix.Data;
using CineTix.Models.Entity_Classes;

namespace CineTix.Models.Interfaces;

public interface IMovieRepository
{
    //Create
    void Add(NewMovie movie);

    //Read
    Movie GetById(int id);
    IEnumerable<Movie> GetAll();
    
    //Update
    void Update(NewMovie movie);
    
    //Delete
    void Delete(int id);
    
    Movie GetMovieByName(string name);
}