using System.Data;
using CineTix.Data;
using CineTix.Models.Interfaces;
using Microsoft.Data.SqlClient;

namespace CineTix.Models.Repositories;

public class UserRepository:IUserRepository
{
    private readonly string _connectionString;
    public UserRepository()
    {
        _connectionString = @"Server=(localdb)\MSSQLLocalDB";
    }
    public DataTable GetUsers()
    {
        DataTable usersTable = new DataTable();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Admin";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.Fill(usersTable);
        }
        return usersTable;
    }

    public void AddUser(Register register)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string insertQuery = "INSERT INTO Admin (Email, Password, Role,FirstName, LastName) VALUES (@Email, @Password,@Role, @FirstName, @LastName)";
            
            SqlCommand command = new SqlCommand(insertQuery, connection);
            
            
            command.Parameters.AddWithValue("@Email", register.EmailAddress);
            command.Parameters.AddWithValue("@Password", register.Password);
            command.Parameters.AddWithValue("@FirstName", register.FirstName);
            command.Parameters.AddWithValue("@LastName", register.LastName);
            command.Parameters.AddWithValue("@Role", "user");
            command.ExecuteNonQuery();
        }
    }
    
    public void Edit(Register register)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string insertQuery = "UPDATE Admin SET Email = @Email, Password = @Password, FirstName = @FirstName, LastName = @LastName WHERE Email = @Email";
            
            SqlCommand command = new SqlCommand(insertQuery, connection);
            
            
            command.Parameters.AddWithValue("@Email", register.EmailAddress);
            command.Parameters.AddWithValue("@Password", register.Password);
            command.Parameters.AddWithValue("@FirstName", register.FirstName);
            command.Parameters.AddWithValue("@LastName", register.LastName);
            //command.Parameters.AddWithValue("@Id", register.Id);
            
            command.ExecuteNonQuery();
        }
    }
    
    public void Delete(string email)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string deleteQuery = "DELETE FROM Admin WHERE Email = @Email";
        
            SqlCommand command = new SqlCommand(deleteQuery, connection);
        
            command.Parameters.AddWithValue("@Email", email);
        
            command.ExecuteNonQuery();
        }
    }

}