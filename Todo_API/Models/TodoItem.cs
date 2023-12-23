namespace Todo_API.Models
{
    public class TodoItem
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DeadLine { get; set; }
        public bool IsDone { get; set; }

        public TodoItem()
        {

        }
        public TodoItem(int iD, string text, string description, DateTime date, bool isDone)
        {
            ID = iD;
            Title = text;
            Description = description;
            DeadLine = date;
            IsDone = isDone;
        }
    }
}
