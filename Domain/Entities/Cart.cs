using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Cart : BaseEntity
    {
        public int UserId { get; private set; }

        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items;

        private Cart() { }

        public Cart(int userId)
        {
            UserId = userId;
        }

        public void AddItem(int productId, int quantity, decimal unitPrice, string productName)
        {
            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem is not null)
            {
                _items.Remove(existingItem);
                var updatedItem = new CartItem(productId, existingItem.Quantity + quantity, unitPrice, productName);
                _items.Add(updatedItem);
            }
            else
            {
                _items.Add(new CartItem(productId, quantity, unitPrice, productName));
            }
        }

        public void RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item is not null)
            {
                _items.Remove(item);
            }
        }

        public void Clear()
        {
            _items.Clear();
        }

        public decimal GetTotalPrice()
            => _items.Sum(i => i.TotalPrice);
    }
}
