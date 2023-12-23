using Microsoft.EntityFrameworkCore;
using Todo_API.Models;

namespace Todo_API.Data
{
    public class TodoContex : DbContext
    {
        public TodoContex(DbContextOptions<TodoContex> options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
