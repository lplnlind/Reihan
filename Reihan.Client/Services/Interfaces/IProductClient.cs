﻿using Reihan.Shared.DTOs;

namespace Reihan.Client.Services
{
    public interface IProductClient
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task CreateAsync(ProductDto dto);
        Task UpdateAsync(ProductDto dto);
        Task DeleteAsync(int id);
        Task<List<ProductDto>> GetLatestAsync();
        Task<List<ProductDto>> FilterAsync(int? categoryId = null);
        Task<List<ProductDto>> SpecialSalesAsync();
        Task<bool> IsInCartAsync(int productId);
        Task SetActiveStatusAsync(int id, bool isActive);
    }
}
