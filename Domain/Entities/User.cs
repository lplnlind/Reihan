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

        // نقش کاربر (مشتری / مدیر)
        public UserRole Role { get; set; }

        // ارتباط با سفارشات و آدرس
        public List<Order> Orders { get; set; } = new();
        public Address Address { get; set; } = null!;

        public void UpdateProfile(string fullName, string email, Address address)
        {
            FullName = fullName;
            Email = new Email(email);
            Address = address;
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
