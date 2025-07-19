namespace Domain.ValueObjects
{
    public class CartItem
    {
        public int ProductId { get; private set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; } 

        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        private CartItem() { }

        public CartItem(CartItem cart)
        {
            ProductId = cart.ProductId;
            Quantity = cart.Quantity;
            UnitPrice = cart.UnitPrice;
            ProductName = cart.ProductName;
            ProductImage = cart.ProductImage;
        }

        public void ChangeQuantity(int quantity) =>
            Quantity = quantity;

        public decimal TotalPrice => Quantity * UnitPrice;

        //protected override IEnumerable<object> GetEqualityComponents()
        //{
        //    yield return ProductId;
        //}
    }
}
