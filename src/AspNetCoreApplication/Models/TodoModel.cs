using System.ComponentModel.DataAnnotations;

namespace AspNetCoreApplication.Models
{
    public class TodoModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsDone { get; set; }
    }
}
