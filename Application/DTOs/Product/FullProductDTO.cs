﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dom = Domain.Models;

namespace Application.DTOs.Product
{
    public class FullProductDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid product ID.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public required string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public List<DetailInformation> DetailInformation { get; set; } = new();
    }
}
