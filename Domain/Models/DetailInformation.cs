using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class DetailInformation
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Text is required.")]
        [StringLength(500, ErrorMessage = "Text cannot exceed 500 characters.")]
        public string Text { get; set; } = string.Empty;
        [ForeignKey("ProductDetail")]
        public int ProductDetailId { get; set; }
    }
}
