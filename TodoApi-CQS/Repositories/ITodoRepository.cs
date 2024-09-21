using TodoApi_CQS.Models;

namespace TodoApi_CQS.Repositories
{
    public interface ITodoRepository
    {
        void Add(string title);
        void Update(TodoItem todoItem);
        void Delete(int id);
        TodoItem GetById(int id);
        IEnumerable<TodoItem> GetAll();
    }
}
