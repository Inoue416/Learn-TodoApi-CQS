using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoApi_CQS.Services
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Initialize()
        {
            CreateTodoItemsTable();
            InsertDummyData();
        }

        private void CreateTodoItemsTable()
        {
            using (var connection = new SqliteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS TodoItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    IsCompleted INTEGER NOT NULL
                )";

                command.ExecuteNonQuery();
            }
        }

        private void InsertDummyData()
        {
            var dummyItems = new List<(string Title, bool IsCompleted)>
        {
            ("Buy groceries", false),
            ("Finish project report", false),
            ("Call mom", false),
            ("Go for a run", true),
            ("Read a book", false)
        };

            using (var connection = new SqliteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();
                var tran = connection.BeginTransaction();
                foreach (var item in dummyItems)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                    INSERT INTO TodoItems (Title, IsCompleted)
                    VALUES (@title, @isCompleted)";

                    command.Parameters.AddWithValue("@title", item.Title);
                    command.Parameters.AddWithValue("@isCompleted", item.IsCompleted ? 1 : 0);

                    command.ExecuteNonQuery();
                }
                tran.Commit();
            }
        }
    }
}
