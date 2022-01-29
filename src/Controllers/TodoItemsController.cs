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
        /// ��������� ��� ������
        /// </summary>
        /// <returns>������ �����</returns>
        /// <response code="200">������ �����</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _todoItemService.GetTodoItemsAsync();
        }

        /// <summary>
        /// ��������� ������ �� ��������������
        /// </summary>
        /// <param name="id">������������� ������</param>
        /// <returns>������ � �������� ���������������</returns>
        /// <response code="200">������ �������</response>
        /// <response code="404">������ �� �������</response>
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
        /// �������� ������
        /// </summary>
        /// <param name="id">������������� ������</param>
        /// <param name="todoItemData">���������� ��� ����������</param>
        /// <returns></returns>
        /// <response code="204">������ ��������</response>
        /// <response code="404">������ �� �������</response>
        /// <response code="400">������������ ������ ������</response>
        [HttpPut("{id}")]
        [ExceptionHandler(typeof(ArgumentOutOfRangeException), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemData todoItemData)
        {
            await _todoItemService.UdpateTodoItemAsync(id, todoItemData.Name, todoItemData.IsComplete);

            return NoContent();
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="request"></param>
        /// <returns>��������� ������</returns>
        /// <response code="201">����� ������</response>
        /// <response code="400">������������ ������ ������</response>
        [HttpPost]
        [ExceptionHandler(typeof(ArgumentException), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemData request)
        {
            var todoItem = await _todoItemService.CreateTodoItemAsync(request.Name, request.IsComplete);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="id">������������� ������</param>
        /// <returns></returns>
        /// <response code="204">������ �������</response>
        /// <response code="404">������ �� �������</response>
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
