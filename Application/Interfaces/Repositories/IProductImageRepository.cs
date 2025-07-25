﻿using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Interfaces.Repositories
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId);
        Task<IEnumerable<ProductImage>> GetByProductIdsAsync(List<int> ids);
        Task AddRangeAsync(IEnumerable<ProductImage> images);
        Task DeleteByProductIdAsync(int productId);
        Task DeleteRangeAsync(IEnumerable<ProductImage> images);
    }
}
