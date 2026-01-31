using System.ComponentModel.DataAnnotations;

namespace TaskManger.API.Models
{
    public class Log
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Action { get; set; } = string.Empty;
        [Required]
        [MaxLength(45)]
        public string IpAddress { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
