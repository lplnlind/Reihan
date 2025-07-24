using Reihan.Shared.DTOs;

namespace Application.Interfaces
{
    public interface IUserAddressService
    {
        Task<List<UserAddressDto>> GetUserAddressesAsync(int userId);
        Task AddAddressAsync(int userId, UserAddressDto dto);
        Task DeleteAsync(int id);
        Task SetDefaultAsync(int userId, int addressId);
        Task<UserAddressDto> GetDefaultAddressAsync(int userId);
    }
}
