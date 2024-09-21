using TodoApi_CQS.Models;
using TodoApi_CQS.Repositories;

namespace TodoApi_CQS.Services.Queries
{
    public class QueryService : IQueryService
    {
        private readonly ITodoRepository _todoRepository;

        public QueryService(ITodoRepository todoRepository) 
        {
            _todoRepository = todoRepository;
        }

        public TodoItem GetTodoById(int id)
        {
            return _todoRepository.GetById(id);
        }

        public IEnumerable<TodoItem> GetTodoAll()
        {
            return _todoRepository.GetAll();
        }
    }
}
