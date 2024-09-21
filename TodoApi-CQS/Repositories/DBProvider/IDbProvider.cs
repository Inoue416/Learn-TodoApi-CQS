using Microsoft.Data.Sqlite;

namespace TodoApi_CQS.Repositories.DBProvider
{
    public interface IDbProvider
    {
        void Open();
        void Close();
        void BeginTransaction();
        void Commit();
        void Rollback();
        SqliteConnection GetConnection();
        void Dispose();
    }
}
