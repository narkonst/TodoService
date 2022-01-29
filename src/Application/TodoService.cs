using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApiDTO.Application.Models;
using TodoApiDTO.Domain;

namespace TodoApiDTO.Application
{
    internal class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<List<TodoItemDTO>> GetTodoItemsAsync()
        {
            var todoItems = await _todoRepository.GetTodoItemsAsync();

            return todoItems.Select(ItemToDTO).ToList();
        }

        public async Task<TodoItemDTO> GetTodoItemAsync(long id)
        {
            var todoItem = await _todoRepository.GetTodoItemAsync(id);

            return todoItem is null ? null : ItemToDTO(todoItem);
        }

        public async Task<TodoItemDTO> CreateTodoItemAsync(string name, bool isComplete)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Не указана задача", nameof(name));
            }

            var todoItem = new TodoItem(name, isComplete);

            await _todoRepository.AddTodoItemAsync(todoItem);

            return new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
        }

        public async Task UdpateTodoItemAsync(long id, string name, bool isComplete)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Не указана задача", nameof(name));
            }

            var todoItem = await _todoRepository.GetTodoItemAsync(id);
            if (todoItem == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Задача не найдена");
            }

            todoItem.ChangeItem(name, isComplete);

            await _todoRepository.UpdateTodoItemAsync(todoItem);
        }

        public async Task DeleteTodoItemAsync(long id)
        {
            var todoItem = await _todoRepository.GetTodoItemAsync(id);
            if (todoItem == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Задача не найдена");
            }
            await _todoRepository.DeleteTodoItemAsync(todoItem);
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}
