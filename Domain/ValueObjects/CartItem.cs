namespace Domain.ValueObjects
{
    public class CartItem
    {
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }

        private CartItem() { }

        public CartItem(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public void ChangeQuantity(int quantity) =>
            Quantity = quantity;
    }
}
