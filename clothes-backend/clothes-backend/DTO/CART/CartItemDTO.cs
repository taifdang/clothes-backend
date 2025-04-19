﻿using clothes_backend.Service;
using clothes_backend.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace clothes_backend.DTO.CART
{
    public class CartItemDTO
    {
        public int? id { get; set; }
        [Required(ErrorMessage = "Product variant id is not empty")]
        public int product_variant_id { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Quantity cart item must be between 1 and 100")]
        public int quantity { get; set; }
        public byte[]? row_version {get;set;}

    }
}
