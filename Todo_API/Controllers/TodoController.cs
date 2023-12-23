using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_API.Data;
using Todo_API.Models;

namespace Todo_API.Controllers
{
    [ApiController]
    [Route("/notes")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContex dataBase;

        public TodoController(TodoContex dataBase)
        {
            this.dataBase = dataBase;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes([FromQuery] bool? completed)
        {
            if (completed.HasValue)
            {
                var notes = await dataBase.TodoItems.Where(x => x.IsDone == completed).ToListAsync();
                return Ok(notes);
            }
            else
            {
                var notes = await dataBase.TodoItems.ToListAsync();
                return Ok(notes);
            }
        }

        [HttpGet]
        [Route("/remaining")]
        public Task<int> Remaining()
        {
            return dataBase.TodoItems.CountAsync(count => !count.IsDone);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(TodoItem item)
        {

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }


            await dataBase.TodoItems.AddAsync(item);
            await dataBase.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPost]
        [Route("/toggle-all")]
        public async Task<IActionResult> ToggelAll()
        {
            var items = await dataBase.TodoItems.ToListAsync();
            var isAllDone = items.Any(notes => !notes.IsDone);

            foreach (var item in items)
            {
                item.IsDone = isAllDone;
            }

            await dataBase.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ChangeValueOfCheckbox(int id)
        {
            var item = dataBase.TodoItems.First(x => x.ID == id);

            if (!item.IsDone)
            {
                item.IsDone = true;
            }
            else
            {
                item.IsDone = false;
            }

            await dataBase.SaveChangesAsync();

            return Ok(item);
        }

        [HttpPost]
        [Route("/clear-completed")]
        public async Task<IActionResult> ClearAll()
        {
            var items = dataBase.TodoItems.Where(x => x.IsDone);

            foreach (var item in items)
            {
                dataBase.TodoItems.Remove(item);
            }
            await dataBase.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var item = dataBase.TodoItems.First(x => x.ID == id);
            dataBase.TodoItems.Remove(item);
            await dataBase.SaveChangesAsync();
            return Ok(item);
        }
    }
}
