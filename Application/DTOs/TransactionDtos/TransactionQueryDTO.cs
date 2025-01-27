using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TransactionDtos
{
    public class TransactionQueryDTO
    {
        public required string UserName { get; set; }
        public int storeId { get; set; }
        public List<CartItem> cartItems { get; set; } = new List<CartItem>();
        public DateTime TransactionDate { get; set; }
    }
}
