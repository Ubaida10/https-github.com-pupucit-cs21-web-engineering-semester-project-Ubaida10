namespace CineTix.Models.Interfaces;

public interface IGenericRepository<T> where T : class
{
    //Create
    void Add(T entity);
    
    //Read
    T GetById(int id);
    IEnumerable<T> GetAll();
    
    //Update
    void Update(T entity);
    
    //Delete
    void Delete(int id);
}