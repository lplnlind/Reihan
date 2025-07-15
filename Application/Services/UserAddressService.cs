using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUserAddressRepository _repo;

        public UserAddressService(IUserAddressRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserAddressDto>> GetUserAddressesAsync(int userId)
        {
            var list = await _repo.GetByUserIdAsync(userId);
            return list.Select(a => new UserAddressDto
            {
                Id = a.Id,
                Title = a.Title,
                State = a.State,
                Street = a.Street,
                City = a.City,
                ZipCode = a.ZipCode,
                IsDefault = a.IsDefault
            }).ToList();
        }

        public async Task AddAddressAsync(int userId, UserAddressDto dto)
        {
            var address = new UserAddress(userId, dto.Title, dto.State, dto.Street, dto.City, dto.ZipCode, dto.IsDefault);
            
            var isExists = await _repo.ExistsAsync(userId);
            if (!isExists)
                address.SetAsDefault();

            await _repo.AddAsync(address);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task SetDefaultAsync(int userId, int addressId)
        {
            var addresses = await _repo.GetByUserIdAsync(userId);

            if (!addresses.Any(a => a.Id == addressId))
                throw new Exception("آدرس معتبر نیست یا متعلق به شما نیست.");

            foreach (var address in addresses)
            {
                if (address.Id == addressId)
                    address.SetAsDefault();

                else
                    address.RemoveDefault();
            }

            await _repo.UpdateRangeAsync(addresses);
        }

        public async Task<UserAddressDto> GetDefaultAddressAsync(int userId)
        {
            var address = await _repo.GetDefaultAddressAsync(userId);

            if (address is null)
                throw new Exception("آدرس پیدا نشد.");

            return new UserAddressDto
            {
                Id = address.Id,
                Title = address.Title,
                State = address.State,
                Street = address.Street,
                City = address.City,
                ZipCode = address.ZipCode,
                IsDefault = address.IsDefault
            };
        }
    }
}
