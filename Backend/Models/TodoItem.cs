using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    /** TodoItem dient wie das Model in der Android App als Datenklasse.
     * Es enthält die Eigenschaften Id, Title, Description und IsComplete.
     */
    public class TodoItem
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime Time { get; set; }
        public bool IsComplete { get; set; }
    }
}
