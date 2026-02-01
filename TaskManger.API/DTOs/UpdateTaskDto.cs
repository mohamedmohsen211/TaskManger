using System.ComponentModel.DataAnnotations;

namespace TaskManger.API.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        public string status { get; set; } = "Pending";
    }
}
