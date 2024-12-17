using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = "";

        public List<Book> Books { get; set; } = new List<Book>(); // Relationship with Books
    }
}
