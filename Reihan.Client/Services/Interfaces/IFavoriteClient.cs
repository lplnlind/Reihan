﻿using Reihan.Shared.DTOs;

namespace Reihan.Client.Services
{
    public interface IFavoriteClient
    {
        Task<List<ProductDto>> GetUserFavoritesAsync();
        Task<bool> IsFavoriteAsync(int productId);
        Task AddToFavoriteAsync(int productId);
        Task RemoveFromFavoriteAsync(int productId);
    }
}
