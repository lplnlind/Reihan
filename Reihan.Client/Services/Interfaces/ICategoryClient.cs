using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface ICategoryClient
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task CreateAsync(CategoryDto dto);
        Task UpdateAsync(CategoryDto dto);
        Task DeleteAsync(int id);
    }
}
