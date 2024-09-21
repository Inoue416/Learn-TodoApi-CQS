using TodoApi_CQS.Models;
using TodoApi_CQS.Repositories;

namespace TodoApi_CQS.Services.Commands
{
    public class CommandService : ICommandService
    {
        private readonly ITodoRepository _todoRepository;
        public CommandService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public void AddTodoItem(string title)
        {
            _todoRepository.Add(title);
        }

        public void UpdateTodoItem(TodoItem item)
        {
            _todoRepository.Update(item);
        }
        public void DeleteTodoItem(int id)
        {
            _todoRepository.Delete(id);
        }
    }
}
