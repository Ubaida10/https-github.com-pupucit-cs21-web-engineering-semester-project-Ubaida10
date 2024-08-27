using CineTix.Models.Interfaces;
using Microsoft.Data.SqlClient;

namespace CineTix.Models.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        protected readonly string _connectionString;

        public GenericRepository()
        {
            _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eTicketProject;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"; ;
        }

        public IEnumerable<T> GetAll()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            string query = $"SELECT * FROM {typeof(T).Name}s";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<T> entities = new List<T>();
            while (reader.Read())
            {
                T entity = MapReaderToEntity(reader);
                entities.Add(entity);
            }
            return entities;
        }

        public T GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            string query = $"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            T entity = null;
            if (reader.Read())
            {
                entity = MapReaderToEntity(reader);
            }
            return entity;
        }

        public void Add(T entity)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            string query = GenerateInsertQuery(entity);
            SqlCommand command = new SqlCommand(query, connection);
            AddParameters(command, entity);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Update(T entity)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            string query = GenerateUpdateQuery(entity);
            SqlCommand command = new SqlCommand(query, connection);
            AddParameters(command, entity);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            string query = $"DELETE FROM {typeof(T).Name}s WHERE Id = @Id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }

        private T MapReaderToEntity(SqlDataReader reader)
        {
            T entity = new T();
            var properties = typeof(T).GetProperties();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var prop = properties.FirstOrDefault(p => p.Name == reader.GetName(i));
                if (prop != null && !reader.IsDBNull(i))
                {
                    prop.SetValue(entity, reader.GetValue(i));
                }
            }

            return entity;
        }

        private string GenerateInsertQuery(T entity)
        {
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id").ToArray();
            string columnNames = string.Join(", ", properties.Select(p => p.Name));
            string parameterNames = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            return $"INSERT INTO {typeof(T).Name}s ({columnNames}) VALUES ({parameterNames})";
        }

        private string GenerateUpdateQuery(T entity)
        {
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id").ToArray();
            string setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
            return $"UPDATE {typeof(T).Name}s SET {setClause} WHERE Id = @Id";
        }

        private void AddParameters(SqlCommand command, T entity)
        {
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(entity) ?? DBNull.Value;
                command.Parameters.AddWithValue($"@{prop.Name}", value);
            }
        }
    }
}
