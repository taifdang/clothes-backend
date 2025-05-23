﻿using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class Carts
    {
        public int id { get; set; }
        public string? session_id { get; set; }
        public int? user_id { get; set; }
        [JsonIgnore]
        public Users users { get; set; }
        public ICollection<CartItems> cartItems { get; set; }
    }
}
