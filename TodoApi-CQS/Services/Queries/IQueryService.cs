using TodoApi_CQS.Models;

namespace TodoApi_CQS.Services.Queries
{
    public interface IQueryService
    {
        TodoItem GetTodoById(int id);
        IEnumerable<TodoItem> GetTodoAll();
    }
}
