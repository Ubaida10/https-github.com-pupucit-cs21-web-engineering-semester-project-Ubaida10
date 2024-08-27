using CineTix.Data;
using CineTix.Models.Entity_Classes;
using CineTix.Models.Interfaces;
using Microsoft.Data.SqlClient;

namespace CineTix.Models.Repositories;

public class MovieRepository : GenericRepository<Movie>, IMovieRepository
{
    public MovieRepository():base()
    {
    }

    public void Add(NewMovie movie)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Movies(Title, Synopsis, Duration, ReleaseDate, Price, ImageUrl, RottenTomatoScore, Genre, ProducerID, CinemaID, ActorID, PortraitUrl) VALUES (@Title, @Synopsis, @Duration, @ReleaseDate, @Price, @ImageUrl, @RottenTomatoScore, @Genre, @ProducerID, @CinemaID, @ActorID, @PortraitUrl);SELECT SCOPE_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@Title", movie.Title);
            command.Parameters.AddWithValue("@Synopsis", movie.Synopsis);
            command.Parameters.AddWithValue("@Duration", movie.Duration);
            command.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate);
            command.Parameters.AddWithValue("@Price", movie.Price);
            command.Parameters.AddWithValue("@ImageUrl", movie.ImageUrl);
            command.Parameters.AddWithValue("@RottenTomatoScore", movie.RottenTomatoScore);
            command.Parameters.AddWithValue("@Genre", movie.Genre);
            command.Parameters.AddWithValue("@ProducerID", movie.ProducerId);
            command.Parameters.AddWithValue("@CinemaID", movie.CinemaId);
            command.Parameters.AddWithValue("@ActorID", movie.ActorId);
            command.Parameters.AddWithValue("PortraitUrl", movie.PortraitUrl);
            
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    

    public Movie GetMovieByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Search name cannot be null or empty", nameof(name));
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            Movie movie = null; // Use null initially if no movie is found
            string query = @"SELECT * FROM Movies m WHERE m.Title = @name";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        movie = new Movie
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Synopsis = reader.GetString(2),
                            Duration = reader.GetTimeSpan(3),
                            ReleaseDate = reader.GetDateTime(4),
                            Price = reader.GetDecimal(5),
                            ImageUrl = reader.GetString(6),
                            RottenTomatoScore = reader.GetInt32(7),
                            Genre = (Genre)reader.GetInt32(8),
                            CinemaId = reader.GetInt32(9),
                            ProducerId = reader.GetInt32(10),
                            ActorId = reader.GetInt32(11),
                            PortraitUrl = reader.GetString(12)
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return movie;
        }
    }


    public void Update(NewMovie movie)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"UPDATE Movies SET Title = @Title, Synopsis = @Synopsis, Duration = @Duration, ReleaseDate = @ReleaseDate, Price = @Price, ImageUrl = @ImageUrl, RottenTomatoScore = @RottenTomatoScore, Genre = @Genre, ProducerID = @ProducerID, CinemaID = @CinemaID, ActorID = @ActorID, PortraitUrl = @PortraitUrl WHERE Id = @Id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", movie.Id);
            command.Parameters.AddWithValue("@Title", movie.Title);
            command.Parameters.AddWithValue("@Synopsis", movie.Synopsis);
            command.Parameters.AddWithValue("@Duration", movie.Duration);
            command.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate);
            command.Parameters.AddWithValue("@Price", movie.Price);
            command.Parameters.AddWithValue("@ImageUrl", movie.ImageUrl);
            command.Parameters.AddWithValue("@RottenTomatoScore", movie.RottenTomatoScore);
            command.Parameters.AddWithValue("@Genre", movie.Genre);
            command.Parameters.AddWithValue("@ProducerID", movie.ProducerId);
            command.Parameters.AddWithValue("@CinemaID", movie.CinemaId);
            command.Parameters.AddWithValue("@ActorID", movie.ActorId); 
            command.Parameters.AddWithValue("PortraitUrl", movie.PortraitUrl);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}