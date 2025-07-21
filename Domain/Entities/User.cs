using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;
using System.Data;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public Email Email { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public string PhoneNumber { get; private set; } = string.Empty;

        public UserRole Role { get; set; }

        public List<Order> Orders { get; set; } = new();
        public List<Favorite> Favorites { get; set; } = new();
        public List<UserAddress> Addresses { get; set; } = null!;

        public void UpdateProfile(User user)
        {
            FullName = user.FullName;
            Email = user.Email;
            UserName = user.UserName;
            PhoneNumber = user.PhoneNumber;
            ImageUrl = user.ImageUrl;
            SetUpdated();
        }

        public void ChangePassword(string hashedPassword)
        {
            PasswordHash = hashedPassword;
            SetUpdated();
        }

        public void SetRole(UserRole role)
        {
            Role = role;
            SetUpdated();
        }

        public void SetStatus(bool isActive)
        {
            IsActive = isActive;
            SetUpdated();
        }
    }
}
