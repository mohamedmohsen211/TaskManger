using System.ComponentModel.DataAnnotations;

namespace TaskManger.API.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }= string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
