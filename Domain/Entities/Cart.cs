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

        public void AddItem(CartItem cart)
        {
            var existingItem = _items.FirstOrDefault(i => i.ProductId == cart.ProductId);
            if (existingItem is not null)
            {
                _items.Remove(existingItem);

                var updatedItem = new CartItem(cart.ProductId, cart.Quantity);
                _items.Add(updatedItem);
            }
            else
            {
                _items.Add(new CartItem(cart.ProductId, cart.Quantity));
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

        public void ChangeQuantity(int productId,int quantity)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item is not null)
                item.ChangeQuantity(quantity);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
