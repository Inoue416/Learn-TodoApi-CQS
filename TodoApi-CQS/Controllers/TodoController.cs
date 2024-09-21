using Microsoft.AspNetCore.Mvc;
using TodoApi_CQS.Models;
using TodoApi_CQS.Models.Dto;
using TodoApi_CQS.Services;
using TodoApi_CQS.Services.Commands;
using TodoApi_CQS.Services.Queries;

namespace TodoApi_CQS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ICommandService _commandService;
        private readonly IQueryService _queryService;
        public TodoController(
            ICommandService commandService,
            IQueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        // GET: api/DatabaseInit
        [HttpGet("DatabaseInit")]
        public IActionResult DatabaseInit() // 学習用のために定義している
        {
            var dbInitilizer = new DatabaseInitializer("Your Path"); // TODO: Please Your path
            dbInitilizer.Initialize();
            var response = new CommandResponseDto()
            {
                IsSuccess = true,
                Message = "Success Database Init"
            };
            return Ok(response);
        }

        // GET: api/TodoItems
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetAll()
        {
            var items = _queryService.GetTodoAll();
            return Ok(items);
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetById(int id)
        {
            var item = _queryService.GetTodoById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST: api/TodoItems
        [HttpPost]
        public ActionResult<TodoItem> Add(string title)
        {
            try
            {
                _commandService.AddTodoItem(title);
                var response = new CommandResponseDto()
                {
                    IsSuccess = true,
                    Message = "Success Add Todo"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            try
            {
                _commandService.UpdateTodoItem(item);
                var response = new CommandResponseDto()
                {
                    IsSuccess = true,
                    Message = "Success Add Todo"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _commandService.DeleteTodoItem(id);
                var response = new CommandResponseDto()
                {
                    IsSuccess = true,
                    Message = "Success Add Todo"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
