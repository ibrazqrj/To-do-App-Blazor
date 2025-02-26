using System.ComponentModel.DataAnnotations;

namespace BlazorToDoApp.Services
{
    public class TodoItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters!")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime Time { get; set; }
        public bool IsComplete { get; set; }
        public bool IsGrouped { get; set; } = false;
    }
}
