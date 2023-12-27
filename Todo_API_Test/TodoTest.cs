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
            var note = new TodoItem(1, "title", "description", new DateTime(2023 - 12 - 26), false);
            var controller = new TodoController(_database);

            // Act
            var result = await controller.AddNote(note);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var resultAddedNote = result as OkObjectResult;
            var addedItem = resultAddedNote?.Value as TodoItem;

            Assert.NotNull(addedItem);
            Assert.Equal(note.Title, addedItem.Title);
            Assert.Equal(note.Description, addedItem.Description);
            Assert.Equal(note.DeadLine, addedItem.DeadLine);
            Assert.Equal(note.IsDone, addedItem.IsDone);
        }
        [Fact]
        public async Task Should_Return_Null()
        {
            //Arrange
            TodoItem? note = null;
            var controller = new TodoController(_database);
            
            //Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => controller.AddNote(note));
        }

        [Fact]
        public async Task Should_Delete_Todo()
        {
            //Act
            var note = new TodoItem(1, "title", "description", new DateTime(2023 - 12 - 26), false);
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
        [Fact]
        public async Task Should_Toggle_All()
        {

            //Arrange
            var note1 = new TodoItem(3, "title", "description", new DateTime(2023 - 12 - 26), false);
            var note2 = new TodoItem(4, "title2", "description2", new DateTime(2023 - 12 - 26), false);
            var controller = new TodoController(_database);
            await controller.AddNote(note1);
            await controller.AddNote(note2);

            //Act
            await controller.ToggelAll();

            //Assert
            Assert.True(note1.IsDone);
            Assert.True(note2.IsDone);

            //Test if they are done should result in not done when call again
            //Act
            await controller.ToggelAll();

            //Assert
            Assert.False(note1.IsDone);
            Assert.False(note2.IsDone);
        }

        [Fact]
        public async Task Should_Clear_All_Complete()
        {
            //Arrange
            var controller = new TodoController(_database);
            var note1 = new TodoItem(5, "title", "description", new DateTime(2023 - 12 - 26), false);
            var note2 = new TodoItem(6, "title2", "description2", new DateTime(2023 - 12 - 26), true);
            var note3 = new TodoItem(7, "title3", "description3", new DateTime(2023 - 12 - 26), true);
            await controller.AddNote(note1);
            await controller.AddNote(note2);
            await controller.AddNote(note3);

            //Act
            await controller.ClearAll();

            //Assert
            var itemInDatabaseNote2 = _database.TodoItems.Find(note3.ID);
            Assert.Null(itemInDatabaseNote2);
            var itemInDatabaseNote3 = _database.TodoItems.Find(note3.ID);
            Assert.Null(itemInDatabaseNote3);
        }
        [Fact]
        public async Task Should_Return_Todo_In_Database()
        {
            //Arrange
            var controller = new TodoController(_database);
            var note1 = new TodoItem(8, "title", "description", new DateTime(2023 - 12 - 26), false);
            var note2 = new TodoItem(9, "title2", "description2", new DateTime(2023 - 12 - 26), true);
            var note3 = new TodoItem(10, "title3", "description3", new DateTime(2023 - 12 - 26), true);
            await controller.AddNote(note1);
            await controller.AddNote(note2);
            await controller.AddNote(note3);

            //Act
           var result = await controller.GetNotes(true);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var notes = okResult.Value as List<TodoItem>; 
            Assert.NotNull(notes);
            Assert.Equal(2, notes.Count); // Check so the list is actual 2
            Assert.Equal(notes[0].Title, note2.Title); //Check so the note on index 1 is the same as 2. 
        }
    }
}