using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUserAddressRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserAddressService> _logger;

        public UserAddressService(IUserAddressRepository repo, IMapper mapper, ILogger<UserAddressService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<UserAddressDto>> GetUserAddressesAsync(int userId)
        {
            var list = await _repo.GetByUserIdAsync(userId);
            return _mapper.Map<List<UserAddressDto>>(list);
        }

        public async Task AddAddressAsync(int userId, UserAddressDto dto)
        {
            var address = _mapper.Map<UserAddress>(dto);
            address.SetUserId(userId);

            var isExists = await _repo.ExistsAsync(userId);
            if (!isExists)
                address.SetAsDefault();

            await _repo.AddAsync(address);
            _logger.LogInformation("آدرس جدید برای کاربر {UserId} با عنوان '{Title}' افزوده شد.", 
                userId, dto.Title);
        }

        public async Task DeleteAsync(int id)
        {
            var address = await _repo.GetByIdAsync(id);

            if (address == null)
            {
                _logger.LogWarning("تلاش برای حذف آدرس نامعتبر با شناسه {AddressId}", id);
                throw new AppException("آدرس یافت نشد.", StatusCodes.Status404NotFound, 
                    ErrorCode.UserAddressNotFound);
            }

            await _repo.DeleteAsync(id);
            _logger.LogInformation("آدرس با شناسه {AddressId} حذف شد.", id);
        }

        public async Task SetDefaultAsync(int userId, int addressId)
        {
            var addresses = await _repo.GetByUserIdAsync(userId);

            if (!addresses.Any(a => a.Id == addressId))
            {
                _logger.LogWarning("کاربر با شناسه {UserId} آدرس پیش‌فرض ندارد{addressId}.", addressId, userId);
                throw new AppException("آدرس معتبر نیست یا متعلق به شما نیست.", 
                    StatusCodes.Status401Unauthorized, ErrorCode.AddressNotFoundOrUnauthorized);
            }

            foreach (var address in addresses)
            {
                if (address.Id == addressId)
                    address.SetAsDefault();

                else
                    address.RemoveDefault();
            }

            await _repo.UpdateRangeAsync(addresses);
            _logger.LogInformation("آدرس با شناسه {AddressId} به عنوان آدرس پیش‌فرض برای کاربر {UserId} تنظیم شد.", 
                addressId, userId);
        }

        public async Task<UserAddressDto> GetDefaultAddressAsync(int userId)
        {
            var address = await _repo.GetDefaultAddressAsync(userId);

            if (address is null)
            {
                _logger.LogWarning("خطا در گرفتن آدرس پیشفرض: آدرس پیش فرض برای این کاربر یافت نشد {UserId}", userId);
                throw new AppException("آدرس پیش‌فرض پیدا نشد.", StatusCodes.Status404NotFound, 
                    ErrorCode.DefaultAddressNotFound);
            }

            return _mapper.Map<UserAddressDto>(address);
        }
    }
}
