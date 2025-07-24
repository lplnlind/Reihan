using Reihan.Shared.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProductRepository _productRepo;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepo, 
            IProductRepository productRepo, 
            ILogger<CategoryService> logger)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            var products = await _productRepo.GetAllAsync();

            var productCountByCategory = products
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count());

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ProductsInThisCategory = productCountByCategory.GetValueOrDefault(c.Id, 0)
            });
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category is null)
                throw new AppException("دسته بندی مورد نظر یافت نشد.", 
                    StatusCodes.Status404NotFound, 
                    ErrorCode.CategoryNotFound);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            var existing = await _categoryRepo.GetByNameAsync(categoryDto.Name);
            if (existing is not null)
                throw new AppException("دسته بندی وارد شده موجود است.", 
                    StatusCodes.Status409Conflict, 
                    ErrorCode.CategoryAlreadyExists);

            var category = new Category
            {
                Name = categoryDto.Name
            };

            await _categoryRepo.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _categoryRepo.GetByIdAsync(categoryDto.Id);

            if (category is null)
                throw new AppException("دسته بندی مورد نظر یافت نشد.", 
                    StatusCodes.Status404NotFound, 
                    ErrorCode.CategoryNotFound);

            category.Name = categoryDto.Name;
            await _categoryRepo.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category is null)
                throw new AppException("دسته بندی مورد نظر یافت نشد", 
                    StatusCodes.Status404NotFound, 
                    ErrorCode.CategoryNotFound);

            await _categoryRepo.DeleteAsync(category.Id);
        }
    }
}
