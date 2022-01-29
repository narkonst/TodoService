using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApiDTO.Domain
{
    internal interface ITodoRepository
    {
        public Task<List<TodoItem>> GetTodoItemsAsync();

        Task<TodoItem> GetTodoItemAsync(long id);

        Task AddTodoItemAsync(TodoItem todoItem);

        Task UpdateTodoItemAsync(TodoItem todoItem);

        Task DeleteTodoItemAsync(TodoItem item);
    }
}
