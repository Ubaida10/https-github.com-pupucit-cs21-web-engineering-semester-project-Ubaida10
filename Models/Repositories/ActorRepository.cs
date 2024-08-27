using CineTix.Data;
using CineTix.Models.Entity_Classes;
using CineTix.Models.Interfaces;
using Microsoft.Data.SqlClient;

namespace CineTix.Models.Repositories
{
    public class ActorRepository : GenericRepository<Actor>, IActorRepository
    {
        public ActorRepository() : base()
        {
        }

        public IEnumerable<Movie> GetMoviesByActor(int id)
        {
            List<Movie> movies = new List<Movie>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT m.* FROM Movies m 
                                 JOIN MovieActors ma ON m.Id = ma.MovieId 
                                 WHERE ma.ActorId = @ActorId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActorId", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Movie m = new Movie
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Synopsis = reader.GetString(2),
                        Duration = reader.GetTimeSpan(3),
                        ReleaseDate = reader.GetDateTime(4),
                        Price = reader.GetDecimal(5),
                        ImageUrl = reader.GetString(6),
                        RottenTomatoScore = reader.GetDouble(7),
                        Genre = (Genre)reader.GetInt32(8)
                    };
                    movies.Add(m);
                }
                connection.Close();
            }
            return movies;
        }
    }
}