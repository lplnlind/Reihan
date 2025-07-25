﻿using Reihan.Shared.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepos, IMapper mapper, IProductRepository productRepo)
        {
            _cartRepo = cartRepos;
            _mapper = mapper;
            _productRepo = productRepo;
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId) ?? new Cart(userId);


            var cartDto = new CartDto
            {
                UserId = userId,
                Items = new List<CartItemDto>()
            };

            foreach (var item in cart.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product == null) continue;

                var finalPrice = product.GetPriceAfterDiscount();

                cartDto.Items.Add(new CartItemDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImage = product.ImageUrl ?? "",
                    UnitPrice = product.Price,
                    FinalPrice = finalPrice,
                    Quantity = item.Quantity
                });
            }

            return cartDto;
        }

        public async Task AddItemAsync(int userId, AddToCartRequest request)
        {
            if (request.Quantity <= 0)
                throw new AppException("تعداد محصول معتبر نیست.", 
                    StatusCodes.Status400BadRequest, 
                    ErrorCode.InvalidProductQuantity);

            var cart = await _cartRepo.GetByUserIdAsync(userId) ?? new Cart(userId);
            cart.AddItem(_mapper.Map<CartItem>(request));

            if (cart.Id == 0)
                await _cartRepo.AddAsync(cart);
            else
                await _cartRepo.UpdateAsync(cart);
        }

        public async Task RemoveItemAsync(int userId, int productId)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId);
            if (cart is null)
                throw new AppException("سبد خرید یافت نشد.", 
                    StatusCodes.Status404NotFound, 
                    ErrorCode.CartNotFound);

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item is null)
                throw new AppException("محصول مورد نظر در سبد خرید وجود ندارد.", 
                    StatusCodes.Status404NotFound, 
                    ErrorCode.CartItemNotFound);

            cart.RemoveItem(productId);
            await _cartRepo.UpdateAsync(cart);
        }

        public async Task ChangeQuantityAsync(int userId, int productId, int quantity)
        {
            if (quantity < 0)
                throw new AppException("تعداد محصول معتبر نیست.", 
                    StatusCodes.Status400BadRequest, 
                    ErrorCode.InvalidProductQuantity);

            var cart = await _cartRepo.GetByUserIdAsync(userId);
            if (cart is null)
                throw new AppException("سبد خرید یافت نشد.", 
                    StatusCodes.Status404NotFound, 
                    ErrorCode.CartNotFound);

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item is null)
                throw new AppException("محصول مورد نظر در سبد خرید وجود ندارد.", 
                    StatusCodes.Status404NotFound, 
                    ErrorCode.CartItemNotFound);

            if (quantity == 0)
            {
                await RemoveItemAsync(userId, productId);
                return;
            }

            cart.ChangeQuantity(productId, quantity);
            await _cartRepo.UpdateAsync(cart);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId);
            if (cart is null)
                throw new AppException("سبد خرید یافت نشد.", StatusCodes.Status404NotFound, ErrorCode.CartNotFound);

            cart.Clear();
            await _cartRepo.UpdateAsync(cart);
        }
    }
}
