namespace Todo_API.Models
{
    public class TodoItem
    {
        public int ID { get; set; }
        public string? Text { get; set; }
        public bool IsDone { get; set; }

        public TodoItem()
        {

        }
        public TodoItem(int iD, string text, bool isDone)
        {
            ID = iD;
            Text = text;
            IsDone = isDone;
        }
    }
}
