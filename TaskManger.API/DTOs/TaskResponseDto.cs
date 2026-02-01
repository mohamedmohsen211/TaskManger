namespace TaskManger.API.DTOs
{
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public string title { get; set; } = string.Empty;
        public string? description { get; set; }
        public string status { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
    }
}
