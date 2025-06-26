namespace Domain.ValueObjects
{
    public class CartItem
    {
        public int ProductId { get; private set; }
        public string ProductName { get; set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        private CartItem() { }

        public CartItem(int productId, int quantity, decimal unitPrice, string productName)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            ProductName = productName;
        }

        public decimal TotalPrice => Quantity * UnitPrice;

        //protected override IEnumerable<object> GetEqualityComponents()
        //{
        //    yield return ProductId;
        //}
    }
}
