﻿using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces
{
    public interface ICartServiceOld
    {
        Task<Result<CartItemDTO>> addCartItem([FromForm] cartAddDTO DTO);
        Task<Result<CartItems>> removeCartItem([FromForm] cartDeleteDTO DTO);
        Task<Result<CartItemDTO>> updateCartItem([FromForm]CartItemDTO DTO);
        Task<Result<List<CartItemDTO>>> getCart();
        //void mergeCart();
    }
}
