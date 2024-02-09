namespace TodoWebService.Models.DTOs.Todo
{
    public class TodoItemDto(int id, string text, bool isCompleted, DateTime createdTime)
    {
        public int Id { get; set; } = id;
        public string Text { get; set; } = text;
        public bool IsCompleted { get; set; } = isCompleted;
        public DateTime CreatedTime { get; set; } = createdTime;
    }
}
