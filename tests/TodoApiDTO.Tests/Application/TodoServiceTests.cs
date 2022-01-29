using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApiDTO.Application;
using TodoApiDTO.Domain;

namespace TodoApiDTO.Tests
{
    [TestFixture(Category = "Unit", TestOf = typeof(TodoService))]
    public class Tests
    {
        [Test]
        public async Task GetTodoItemsAsync_SholdCorrectCallTodoRepository()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemsAsync()).ReturnsAsync(new List<TodoItem>());
            var service = new TodoService(todoRepository.Object);

            await service.GetTodoItemsAsync();

            todoRepository.Verify(r => r.GetTodoItemsAsync(), Times.Once);
        }

        [Test]
        public async Task GetTodoItemsAsync_NoItemsInRepository_ReturnsEmptyResult()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemsAsync()).ReturnsAsync(new List<TodoItem>());
            var service = new TodoService(todoRepository.Object);

            var result = await service.GetTodoItemsAsync();

            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetTodoItemsAsync_HasItemsInRepository_ReturnsCorrectResult()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemsAsync()).ReturnsAsync(new List<TodoItem> 
            { 
                new TodoItem("Do", false)
            });
            var service = new TodoService(todoRepository.Object);

            var result = await service.GetTodoItemsAsync();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, result.Count);
                var expectedItem = result[0];
                Assert.AreEqual("Do", expectedItem.Name);
                Assert.IsFalse(expectedItem.IsComplete);
            });
        }

        [Test]
        public async Task GetTodoItemAsync_SholdCorrectCallTodoRepository()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemAsync(It.IsAny<long>())).ReturnsAsync(default(TodoItem));
            var service = new TodoService(todoRepository.Object);

            await service.GetTodoItemAsync(2);

            todoRepository.Verify(r => r.GetTodoItemAsync(2), Times.Once);
        }

        [Test]
        public async Task GetTodoItemAsync_NoItem_ReturnsNull()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemAsync(It.IsAny<long>())).ReturnsAsync(default(TodoItem));
            var service = new TodoService(todoRepository.Object);

            var result = await service.GetTodoItemAsync(2);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetTodoItemAsync_HasItem_ReturnsCorrectResult()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemAsync(It.IsAny<long>())).ReturnsAsync(new TodoItem("Do", false));
            var service = new TodoService(todoRepository.Object);

            var result = await service.GetTodoItemAsync(2);

            Assert.IsNotNull(result);
            Assert.AreEqual("Do", result.Name);
            Assert.IsFalse(result.IsComplete);
        }

        [TestCase(null)]
        [TestCase("  ")]
        public void CreateTodoItemAsync_NameNotFilled_Throws(string name)
        {
            var service = new TodoService(Mock.Of<ITodoRepository>());

            AsyncTestDelegate createTodo = async () => await service.CreateTodoItemAsync(name, false);

            Assert.ThrowsAsync<ArgumentException>(createTodo);
        }

        [Test]
        public async Task CreateTodoItemAsync_SholdCorrectCallTodoRepository()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.AddTodoItemAsync(It.IsAny<TodoItem>()));
            var service = new TodoService(todoRepository.Object);

            await service.CreateTodoItemAsync("Do", false);

            todoRepository.Verify(r => r.AddTodoItemAsync(It.Is<TodoItem>(x=>x.Name == "Do" && !x.IsComplete)), Times.Once);
        }

        [Test]
        public async Task GetTodoItemAsync_CorrectRequest_ReturnsCorrectResult()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.AddTodoItemAsync(It.IsAny<TodoItem>()));
            var service = new TodoService(todoRepository.Object);

            var result = await service.CreateTodoItemAsync("Do", false);

            Assert.IsNotNull(result);
            Assert.AreEqual("Do", result.Name);
            Assert.IsFalse(result.IsComplete);
        }

        [TestCase(null)]
        [TestCase("  ")]
        public void UdpateTodoItemAsync_NameNotFilled_Throws(string name)
        {
            var service = new TodoService(Mock.Of<ITodoRepository>());

            AsyncTestDelegate updateTodo = async () => await service.UdpateTodoItemAsync(2, name, false);

            Assert.ThrowsAsync<ArgumentException>(updateTodo);
        }


        [Test]
        public void UdpateTodoItemAsync_ItemNotFound_Throws()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemAsync(It.IsAny<long>())).ReturnsAsync(default(TodoItem));
            var service = new TodoService(todoRepository.Object);

            AsyncTestDelegate updateTodo = async () => await service.UdpateTodoItemAsync(2, "Do", false);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(updateTodo);
        }

        [Test]
        public async Task UdpateTodoItemAsync_HasItem_SholdCorrectCallTodoRepository()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemAsync(It.IsAny<long>())).ReturnsAsync(new TodoItem("OldDo", true));
            var service = new TodoService(todoRepository.Object);

           await service.UdpateTodoItemAsync(2, "Do", false);

            todoRepository.Verify(r => r.UpdateTodoItemAsync(It.Is<TodoItem>(x => x.Name == "Do" && !x.IsComplete)), Times.Once);
        }

        [Test]
        public void DeleteTodoItemAsync_ItemNotFound_Throws()
        {
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemAsync(It.IsAny<long>())).ReturnsAsync(default(TodoItem));
            var service = new TodoService(todoRepository.Object);

            AsyncTestDelegate updateTodo = async () => await service.DeleteTodoItemAsync(2);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(updateTodo);
        }

        [Test]
        public async Task DeleteTodoItemAsync_HasItem_SholdCorrectCallTodoRepository()
        {
            var repositoryTodo = new TodoItem("Do", true);
            var todoRepository = new Mock<ITodoRepository>();
            todoRepository.Setup(r => r.GetTodoItemAsync(It.IsAny<long>())).ReturnsAsync(repositoryTodo);
            var service = new TodoService(todoRepository.Object);

            await service.DeleteTodoItemAsync(2);

            todoRepository.Verify(r => r.DeleteTodoItemAsync(repositoryTodo), Times.Once);
        }
    }
}