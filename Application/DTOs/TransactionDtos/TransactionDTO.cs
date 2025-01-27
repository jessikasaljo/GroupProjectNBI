using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
