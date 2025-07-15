using Reihan.Client.Models;

namespace Reihan.Client.Services
{
    public interface IUserAddressClient
    {
        Task<List<UserAddressDto>> GetAllAsync();
        Task CreateAsync(UserAddressDto dto);
        Task DeleteAsync(int id);
        Task SetDefault(int addressId);
        Task<UserAddressDto?> GetDefault();
    }
}
