using CineTix.Models.Entity_Classes;
using CineTix.Models.Interfaces;
using Microsoft.Data.SqlClient;

namespace CineTix.Models.Repositories;

public class CinemaRepository : ICinemaRepository
{
    private readonly string _connectionString;

    public CinemaRepository()
    {
        _connectionString = @"Server=(localdb)\MSSQLLocalDB";
    }

    public void AddCinema(Cinema cinema)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Cinemas(CinemaLogoUrl, Name, Location, Description, ContactInfo) VALUES (@CinemaLogoUrl, @Name, @Location, @Description, @ContactInfo);SELECT SCOPE_IDENTITY()"; 
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", cinema.Id);
                command.Parameters.AddWithValue("@CinemaLogoUrl", cinema.CinemaLogoUrl);
                command.Parameters.AddWithValue("@Name", cinema.Name);
                command.Parameters.AddWithValue("@Location", cinema.Location);
                command.Parameters.AddWithValue("@Description", cinema.Description);
                command.Parameters.AddWithValue("@ContactInfo", cinema.ContactInfo);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    public Cinema GetCinemaById(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            Cinema cinema = new Cinema();
            string query = @"SELECT * FROM Cinemas WHERE Id = @Id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                cinema.Id = reader.GetInt32(0);
                cinema.CinemaLogoUrl = reader.GetString(1);
                cinema.Name = reader.GetString(2);
                cinema.Location = reader.GetString(3);
                cinema.Description = reader.GetString(4);
                cinema.ContactInfo = reader.GetString(5);
            }
            reader.Close();
            connection.Close();
            return cinema;
        }
    }

    public IEnumerable<Movie> GetAllMoviesInACinema(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Cinema> GetAllCinemas()
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = @"SELECT * FROM Cinemas";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Cinema> cinemas = new List<Cinema>();
            while (reader.Read())
            {
                Cinema c = new Cinema
                {
                    Id = reader.GetInt32(0),
                    CinemaLogoUrl = reader.GetString(1),
                    Name = reader.GetString(2),
                    Location = reader.GetString(3),
                    Description = reader.GetString(4),
                    ContactInfo = reader.GetString(5)
                };
                cinemas.Add(c);
            }
            conn.Close();
            return cinemas;
        }
    }

    public void UpdateCinema(Cinema cinema)
    {
        string query = "UPDATE Cinemas SET Name = @Name, Location = @Location, Description = @Description, ContactInfo = @ContactInfo, CinemaLogoUrl = @CinemaLogoUrl WHERE Id = @Id";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", cinema.Id);
            command.Parameters.AddWithValue("@Name", cinema.Name);
            command.Parameters.AddWithValue("@Location", cinema.Location);
            command.Parameters.AddWithValue("@Description", cinema.Description);
            command.Parameters.AddWithValue("@ContactInfo", cinema.ContactInfo);
            command.Parameters.AddWithValue("@CinemaLogoUrl", cinema.CinemaLogoUrl);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void DeleteCinema(Cinema cinema)
    {
        string query = "DELETE FROM Cinemas WHERE Id = @Id";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", cinema.Id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}