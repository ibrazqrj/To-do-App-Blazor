namespace BlazorToDoApp.Services
{
    public interface ITodoService
    {
        Task<List<TodoItem>?> GetTodosAsync(int offset = 0, int limit = 10, bool? isCompleted = null);
        Task<int?> GetTodoCount(bool completed = false);
        Task<bool> CreateTodoAsync(TodoItem newTodo);
        Task<TodoItem?> GetTodoByIdAsync(int id);
        Task<bool> UpdateTodoAsync(TodoItem updatedTodo);
        Task<bool> DeleteTodoAsync(int id);
        Task<bool> DeleteAllBinTodosAsync(bool? isCompleted = true);
    }
}
