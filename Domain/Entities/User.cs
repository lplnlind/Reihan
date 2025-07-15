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
        public Email Email { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public UserRole Role { get; set; }

        public List<Order> Orders { get; set; } = new();
        public List<Favorite> Favorites { get; set; } = new();
        public List<UserAddress> Addresses { get; set; } = null!;

        public void UpdateProfile(string fullName, string email, string userName)
        {
            FullName = fullName;
            Email = new Email(email);
            UserName = userName;
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
