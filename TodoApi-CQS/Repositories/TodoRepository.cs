using Microsoft.Data.Sqlite;
using TodoApi_CQS.Models;
using TodoApi_CQS.Repositories.DBProvider;

namespace TodoApi_CQS.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IDbProvider _dbProvider;
        public TodoRepository(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public void Add(string title)
        {
            var dbConnection = StartTransaction();
            
            EndTransaction();
            string sql = "INSERT INTO TodoItems (Title, IsCompleted) VALUES (@Title, @IsCompleted)";
            try
            {
                _dbProvider.Open();
                _dbProvider.BeginTransaction();

                using (var command = _dbProvider.GetConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@IsCompleted", 0);
                    command.ExecuteNonQuery();
                }

                _dbProvider.Commit();
            }
            catch (Exception)
            {
                _dbProvider.Rollback();
                throw;
            }
            finally
            {
                _dbProvider.Close();
            }
        }

        public void Update(TodoItem todoItem)
        {
            var dbConnection = StartTransaction();

            EndTransaction();
            string sql = "UPDATE TodoItems SET Title = @Title, IsCompleted = @IsCompleted WHERE Id = @Id";
            try
            {
                _dbProvider.Open();
                _dbProvider.BeginTransaction();

                using (var command = _dbProvider.GetConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", todoItem.Id);
                    command.Parameters.AddWithValue("@Title", todoItem.Title);
                    command.Parameters.AddWithValue("@IsCompleted", todoItem.IsCompleted);
                    command.ExecuteNonQuery();
                }

                _dbProvider.Commit();
            }
            catch (Exception)
            {
                _dbProvider.Rollback();
                throw;
            }
            finally
            {
                _dbProvider.Close();
            }

        }

        public void Delete(int id)
        {
            string sql = "DELETE FROM TodoItems WHERE Id = @Id";

            try
            {
                _dbProvider.Open();
                _dbProvider.BeginTransaction();

                using (var command = _dbProvider.GetConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }

                _dbProvider.Commit();
            }
            catch (Exception)
            {
                _dbProvider.Rollback();
                throw;
            }
            finally
            {
                _dbProvider.Close();
            }
        }

        public TodoItem GetById(int id)
        {
            string sql = "SELECT Id, Title, IsCompleted FROM TodoItems WHERE Id = @Id";
            TodoItem item = null;

            try
            {
                _dbProvider.Open();

                using (var command = _dbProvider.GetConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = new TodoItem
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                IsCompleted = reader.GetInt32(2)
                            };
                        }
                    }
                }
            }
            finally
            {
                _dbProvider.Close();
            }

            return item;
        }

        public IEnumerable<TodoItem> GetAll()
        
        {
            string sql = "SELECT Id, Title, IsCompleted FROM TodoItems";
            var items = new List<TodoItem>();

            try
            {
                _dbProvider.Open();

                using (var command = _dbProvider.GetConnection().CreateCommand())
                {
                    command.CommandText = sql;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new TodoItem
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                IsCompleted = reader.GetInt32(2)
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            finally
            {
                _dbProvider.Close();
            }

            return items;
        }

        private SqliteConnection StartTransaction()
        {
            _dbProvider.Open();
            _dbProvider.BeginTransaction();
            return _dbProvider.GetConnection();
        }
        private void EndTransaction() // TODO: フラグでRollbackができるようにする
        {
            _dbProvider.Commit();
            _dbProvider.Close();
            _dbProvider.Dispose();
        }
    }
}
