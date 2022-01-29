using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApiDTO.Domain;

namespace TodoApiDTO.Infrastructure
{
    internal class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _todoContext;

        public TodoRepository(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task AddTodoItemAsync(TodoItem todoItem)
        {
            await _todoContext.TodoItems.AddAsync(todoItem);
            await _todoContext.SaveChangesAsync();
        }

        public Task<TodoItem> GetTodoItemAsync(long id)
        {
            return _todoContext.TodoItems.FindAsync(id).AsTask();
        }

        public Task<List<TodoItem>> GetTodoItemsAsync()
        {
            return _todoContext.TodoItems.ToListAsync();
        }

        public async Task UpdateTodoItemAsync(TodoItem todoItem)
        {
            try
            {
                await _todoContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(todoItem.Id))
            {
                throw new ArgumentOutOfRangeException(nameof(todoItem), "Задача не найдена");
            }
        }

        public Task DeleteTodoItemAsync(TodoItem item)
        {
            _todoContext.TodoItems.Remove(item);

            return _todoContext.SaveChangesAsync();
        }

        private bool TodoItemExists(long id)
        {
            return _todoContext.TodoItems.Any(e => e.Id == id);
        }
    }
}
