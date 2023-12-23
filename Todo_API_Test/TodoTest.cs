using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_API.Controllers;
using Todo_API.Data;
using Todo_API.Models;

namespace Todo_API_Test
{
    public class TodoTest
    {
        private readonly TodoContex _database;
        public TodoTest()
        {
            var inMemoryDatabase = new DbContextOptionsBuilder<TodoContex>()
                .UseInMemoryDatabase("InMemory")
                .Options;
            _database = new TodoContex(inMemoryDatabase);
        }


        [Fact]
        public async Task Should_Return_Todo_Whit_Details()
        {
            // Arrange
            var note = new TodoItem(1, "test", false);
            var controller = new TodoController(_database);

            // Act
            var result = await controller.AddNote(note);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var resultAddedNote = result as OkObjectResult;
            var addedItem = resultAddedNote?.Value as TodoItem;

            Assert.NotNull(addedItem);
            Assert.Equal(note.Text, addedItem.Text);
            

        }
        [Fact]
        public async Task Should_Return_Null()
        {
            TodoItem? note = null;
            var controller = new TodoController(_database);

            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.AddNote(note));
        }

        [Fact]
        public async Task Should_Delete_Todo()
        {
            //Act
            var note = new TodoItem(1, "test", false);
            var controller = new TodoController(_database);

            //Act
            var resultAddedNote = await controller.AddNote(note);
            var addedItem = resultAddedNote as OkObjectResult;
            var resultItem = addedItem?.Value as TodoItem;

            //Get the id of note to remove
            var noteId = resultItem.ID;

            var deleteResult = await controller.DeleteTask(noteId);

            //Assert
            Assert.IsType<OkObjectResult>(deleteResult);

            //check if the notid is in database
            var itemInDatabase = _database.TodoItems.Find(noteId);
            Assert.Null(itemInDatabase);
        }
    }
}