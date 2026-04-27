using System.ComponentModel.DataAnnotations;

namespace WebApiTut.DTO
{
    public class ToDoItemDTO
    {
        public int Id { get; set; }

        // model Validation
        [Required]
        public string Title { get; set; } = string.Empty;
        public bool Completed { get; set; } = false;
    }
}
