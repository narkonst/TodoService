using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApiDTO.Application.Models;

namespace TodoApiDTO.Application
{
    public interface ITodoService
    {
        public Task<List<TodoItemDTO>> GetTodoItemsAsync();

        public Task<TodoItemDTO> GetTodoItemAsync(long id);

        public Task<TodoItemDTO> CreateTodoItemAsync(string name, bool isComplete);

        public Task UdpateTodoItemAsync(long id, string name, bool isComplete);

        public Task DeleteTodoItemAsync(long id);
    }
}
