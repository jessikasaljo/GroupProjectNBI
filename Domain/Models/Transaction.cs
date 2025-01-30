using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public Store Store { get; set; } = null!;
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = null!;
        public DateTime TransactionDate { get; set; }
    }
}
