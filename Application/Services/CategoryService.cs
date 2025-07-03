using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

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
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task AddCategoryAsync(CategoryDto Category)
        {
            var category = new Category
            {
                Name = Category.Name
            };

            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(CategoryDto Category)
        {
            var category = await _categoryRepository.GetByIdAsync(Category.Id);
            if (category is null) return;

            category.Name = Category.Name;
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is not null)
            {
                await _categoryRepository.DeleteAsync(category.Id);
            }
        }
    }
}
