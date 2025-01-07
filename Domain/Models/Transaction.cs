using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CustomerUser")]
        public int CustomerUserId { get; set; }

        [ForeignKey("Store")]
        public int StoreId { get; set; }

        public double Sum { get; set; }
        public DateTime Date { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
