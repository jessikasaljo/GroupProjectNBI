﻿using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.StoreItemDtos
{
    public class UpdateStoreItemDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}