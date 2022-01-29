using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TodoApiDTO.Application;
using TodoApiDTO.Application.Models;
using TodoApiDTO.Filters;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService _todoItemService;

        public TodoItemsController(ITodoService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        /// <summary>
        /// Запросить все задачи
        /// </summary>
        /// <returns>Список задач</returns>
        /// <response code="200">Список задач</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _todoItemService.GetTodoItemsAsync();
        }

        /// <summary>
        /// Запросить задачу по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Задача с заданным идентификатором</returns>
        /// <response code="200">Задача найдена</response>
        /// <response code="404">Задача не найдена</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _todoItemService.GetTodoItemAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        /// <summary>
        /// Изменить задачу
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="todoItemData">информация для обновления</param>
        /// <returns></returns>
        /// <response code="204">Задача изменена</response>
        /// <response code="404">Задача не найдена</response>
        /// <response code="400">Некорректные данные задачи</response>
        [HttpPut("{id}")]
        [ExceptionHandler(typeof(ArgumentOutOfRangeException), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemData todoItemData)
        {
            await _todoItemService.UdpateTodoItemAsync(id, todoItemData.Name, todoItemData.IsComplete);

            return NoContent();
        }

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Созданную задачу</returns>
        /// <response code="201">Новая задача</response>
        /// <response code="400">Некорректный данные задачи</response>
        [HttpPost]
        [ExceptionHandler(typeof(ArgumentException), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemData request)
        {
            var todoItem = await _todoItemService.CreateTodoItemAsync(request.Name, request.IsComplete);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns></returns>
        /// <response code="204">Задача удалена</response>
        /// <response code="404">Задача не найдена</response>
        [HttpDelete("{id}")]
        [ExceptionHandler(typeof(ArgumentOutOfRangeException), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            await _todoItemService.DeleteTodoItemAsync(id);

            return NoContent();
        }

    }

    public class TodoItemData
    {
        [Required]
        public string Name { get; set; }

        public bool IsComplete { get; set; }
    }
}
