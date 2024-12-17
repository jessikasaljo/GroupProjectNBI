using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = "";

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(50, ErrorMessage = "Genre cannot exceed 50 characters.")]
        public string Genre { get; set; } = "";

        [Required(ErrorMessage = "Publication date is required.")]
        public DateTime Date { get; set; }

        public List<Author> Author { get; set; } = new List<Author>(); // Relationship with Authors
    }
}