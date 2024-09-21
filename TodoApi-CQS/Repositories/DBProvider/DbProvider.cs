using Microsoft.Data.Sqlite;

namespace TodoApi_CQS.Repositories.DBProvider
{
    public class DbProvider : IDbProvider
    {
        private const string _dbSourcePath = "YourPath"; // TODO: Please replace you path.
        private SqliteConnection? _connection;
        private SqliteTransaction? _transaction;

        public void Open()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection($"Data Source={_dbSourcePath}");
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void Close()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public void BeginTransaction()
        {
            if (_connection?.State != System.Data.ConnectionState.Open)
            {
                throw new InvalidOperationException("Connection is not open.");
            }

            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No active transaction to commit.");
            }

            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No active transaction to rollback.");
            }

            _transaction.Rollback();
            _transaction = null;
        }

        public SqliteConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
