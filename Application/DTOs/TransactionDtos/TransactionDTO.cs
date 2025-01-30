﻿using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs.TransactionDtos
{
    public class TransactionDTO
    {
        public int StoreId { get; set; }
        public int CartId { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
