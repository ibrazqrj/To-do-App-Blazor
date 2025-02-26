
namespace BlazorToDoApp.Services
{
    public class MemoryTodoService : ITodoService
    {
        private List<TodoItem> _todos = new List<TodoItem>();
        private int _nextId = 1;

        public Task<List<TodoItem>?> GetTodosAsync(int offset = 0, int limit = 10, bool? isCompleted = null)
        {
            var filteredTodos = _todos
                .Where(t => isCompleted == null || t.IsComplete == isCompleted)
                .Skip(offset)
                .Take(limit)
                .ToList();

            return Task.FromResult<List<TodoItem>?>(filteredTodos);
        }
         
        public Task<bool> CreateTodoAsync(TodoItem newTodo)
        {
            newTodo.Id = _nextId++;
            _todos.Add(newTodo);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteTodoAsync(int id)
        {
            _todos.RemoveAll(t => t.Id == id);
            return Task.FromResult(true);
        }

        public Task<TodoItem?> GetTodoByIdAsync(int id)
        {
            return Task.FromResult(_todos.FirstOrDefault(t => t.Id == id));
        }

        public Task<int?> GetTodoCount(bool completed = false)
        {
            return Task.FromResult<int?>(_todos.Count(t => t.IsComplete == completed));
        }

        public Task<bool> UpdateTodoAsync(TodoItem updatedTodo)
        {
            var index = _todos.FindIndex(t => t.Id == updatedTodo.Id);
            if (index == -1)
            {
                return Task.FromResult(false);
            }

            _todos[index] = updatedTodo;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAllBinTodosAsync(bool? isCompleted = true)
        {
            _todos.RemoveAll(t => t.IsComplete == isCompleted);
            return Task.FromResult(true);
        }
    }
}
