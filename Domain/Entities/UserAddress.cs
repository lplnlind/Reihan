using Domain.Common;

namespace Domain.Entities
{
    public class UserAddress : BaseEntity
    {
        public int UserId { get; private set; }
        public string Title { get; private set; } = string.Empty; 
        public string State { get; private set; } = string.Empty;
        public string Street { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string ZipCode { get; private set; } = string.Empty;

        public bool IsDefault { get; private set; } = false;

        public User User { get; set; } = null!;

        public UserAddress(int userId, string title, string state, string street, string city, string zipCode, bool isDefault = false)
        {
            UserId = userId;
            Title = title;
            State = state;
            Street = street;
            City = city;
            ZipCode = zipCode;
            IsDefault = isDefault;
        }

        public void SetAsDefault() => IsDefault = true;
        public void RemoveDefault() => IsDefault = false;

        public void Update(string title, string state, string street, string city, string zipCode)
        {
            Title = title;
            State = state;
            Street = street;
            City = city;
            ZipCode = zipCode;
        }
    }

}
