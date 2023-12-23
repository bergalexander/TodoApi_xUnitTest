namespace Todo_API.Models
{
    public class TodoItem
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }

        public TodoItem()
        {

        }
        public TodoItem(int iD, string text, string description, bool isDone)
        {
            ID = iD;
            Title = text;
            Description = description;  
            IsDone = isDone;
        }
    }
}
