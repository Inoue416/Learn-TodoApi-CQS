using TodoApi_CQS.Models;

namespace TodoApi_CQS.Services.Commands
{
    public interface ICommandService
    {
        void AddTodoItem(string title);
        void UpdateTodoItem(TodoItem todoItem);
        void DeleteTodoItem(int id);
    }
}
