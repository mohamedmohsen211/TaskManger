using System.ComponentModel.DataAnnotations;

namespace TaskManger.API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
