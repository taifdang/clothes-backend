﻿using AutoMapper;
using clothes_backend.Data;
using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.DTO.ORDER;
using clothes_backend.Interfaces;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using clothes_backend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;

namespace clothes_backend.Repository
{
    public class OrderRepositoryOld : BaseRepository<Orders>,IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUserContextService _authService;
        public OrderRepositoryOld(DatabaseContext db, IMapper mapper, IUserContextService authService) : base(db)
        {
            _mapper = mapper;
            _authService = authService;
        }     
        public async Task<Result<Orders>> add([FromForm] orderItemDTO request)
        {
            var user_id = _authService.convertToInt(_authService.getValueAuth());
            if (user_id == 0) return Result<Orders>.Failure(Utils.Enum.StatusCode.Unauthorized);
            //lay danh sach san pham o cartItem           
            var cartItem = await _db.cart_items
                .Include(x => x.product_variants)
                .Where(x => request.cartItem.Contains(x.id) && x.carts.user_id == user_id)
                .ToListAsync(); //tracking
            //kiem tra
            if (!cartItem.Any()) return Result<Orders>.Failure(Utils.Enum.StatusCode.NotFound);
            if (cartItem.Count() != request.cartItem.Count()) return Result<Orders>.Failure(Utils.Enum.StatusCode.NotFound);
            //kiem tra so luong (slban, slmua, slkho)
            //1.lay so luong ban
            //2.lay so luong ton kho
            var getItem = cartItem.Select(x => x.product_variant_id).ToList();
            var _sold = await _db.order_history //slban cua variant co the = null
                .Where(x => getItem.Contains(x.product_variant_id)).ToListAsync();
                //.ToDictionary(x => x.product_variant_id, y => y.sold_quantity);
            var orderHistoriesAdd = new List<OrderHistory>();
            var orderHistoriesUpdate = new List<OrderHistory>();
            //kiem tra
            foreach (var item in cartItem)
            {
                //var sold_quantity = _sold.TryGetValue(item.id, out int value) ? value : 0;
                var sold_quantity = _sold.FirstOrDefault(x => x.product_variant_id == item.product_variant_id)?.sold_quantity ?? 0;
                //if (sold_quantity + item.quantity > item.product_variants.quantity) return Result<List<Orders>>.Failure(Utils.Enum.StatusCode.Isvalid);
                if (sold_quantity + item.quantity > item.product_variants.quantity) return Result<Orders>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
            //dat hang
            //chi tiet don hang
            var orderDetail = _mapper.Map<List<OrderDetails>>(cartItem);//tempory key
            var order = new Orders()
            {
                user_id = user_id,
                note = request.note,
                phone = request.phone,
                address = request.address,
                payment_type = request.payment_type,
                order_details = orderDetail,//=> identity key
                total = orderDetail.Sum(x => x.price)
            };
            _db.orders.Add(order);
            //cap nhat so luong ban
            foreach(var item in cartItem)
            {
                var IsSold = _sold.FirstOrDefault(x=>x.product_variant_id == item.product_variant_id);
                if(IsSold == null)
                {
                    orderHistoriesAdd.Add(new OrderHistory { product_variant_id = item.product_variant_id, sold_quantity = item.quantity });
                }
                else
                {
                    IsSold.sold_quantity += item.quantity;
                    orderHistoriesUpdate.Add(IsSold);
                }           
            }       
            if (orderHistoriesUpdate.Count() > 0) _db.order_history.UpdateRange(orderHistoriesUpdate);
            if (orderHistoriesAdd.Count() > 0) _db.order_history.AddRange(orderHistoriesAdd);           
            //xoa cartItem cu
            _db.cart_items.RemoveRange(cartItem);
            _db.SaveChanges();          
            return Result<Orders>.Success(order);
        }
        public async Task<Result<List<orderDTO>>> getAll()
        {
            var user_id = _authService.convertToInt(_authService.getValueAuth());
            if (user_id == 0) return Result<List<orderDTO>>.Failure(Utils.Enum.StatusCode.Unauthorized);
            var order = await _db.orders.Where(x => x.user_id == user_id).ToListAsync();
            var data = _mapper.Map<List<orderDTO>>(order);
            return Result<List<orderDTO>>.Success(data);   
        }
        public async Task<Result<List<orderDetailDTO>>> getId( int id)
        {
            var user_id = _authService.convertToInt(_authService.getValueAuth());
            if (user_id == 0) return Result<List<orderDetailDTO>>.Failure(Utils.Enum.StatusCode.Unauthorized);
            var order = await _db.order_details.Include(x=>x.orders).Where(x => x.orders.user_id == user_id && x.id == x.orders.id).ToListAsync();
            var data = _mapper.Map<List<orderDetailDTO>>(order);
            return Result<List<orderDetailDTO>>.Success(data);
        }    
    }
}
