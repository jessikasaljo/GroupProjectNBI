using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs.TransactionDtos
{
    public class TransactionDTO
    {
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        [System.Text.Json.Serialization.JsonIgnore]
        public Store Store { get; set; } = null!;
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        [System.Text.Json.Serialization.JsonIgnore]
        public Cart Cart { get; set; } = null!;
        public DateTime TransactionDate { get; set; }
    }
}
