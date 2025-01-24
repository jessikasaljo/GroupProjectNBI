using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class ProductDetail
    {
        [Key, ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public required Product Product { get; set; }
        public ICollection<DetailInformation> DetailInformation { get; set; } = new List<DetailInformation>();
    }
}
