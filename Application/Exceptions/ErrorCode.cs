namespace Application.Exceptions
{
    public enum ErrorCode
    {
        // ----------------------------
        // Common
        // ----------------------------
        Unknown = 0,
        BadRequest = 9000,
        InternalError = 9999,

        // ----------------------------
        // Auth (1000-1099)
        // ----------------------------
        UsernameAlreadyExists = 1001,
        EmailAlreadyExists = 1002,
        InvalidLoginCredentials = 1003,
        UserNotFound = 1004,
        InvalidCurrentPassword = 1005,
        InvalidUserRole = 1006,

        // ----------------------------
        // Product (2000-2099)
        // ----------------------------
        ProductNotFound = 2001,
        ProductAlreadyExists = 2002,
        ProductsNotFound = 2003,
        InvalidProductQuantity = 2004,

        // ----------------------------
        // Cart (3000-3099)
        // ----------------------------
        CartNotFound = 3000,
        CartItemNotFound = 3001,

        // ----------------------------
        // Category (4000-4099)
        // ----------------------------
        CategoryNotFound = 4000,
        CategoryAlreadyExists = 4001,

        // ----------------------------
        // Favorite (5000-5099)
        // ----------------------------
        AlreadyExistsInFavorites = 5000,
        FavoriteItemNotFound = 5001,

        // ----------------------------
        // Order (6000-6099)
        // ----------------------------
        OrderNotFound = 6000,
        InvalidOrderStatus = 6001,
        CartIsEmpty = 6002,
        InsufficientStock = 6003,

        // ----------------------------
        // User Address (7000-7099)
        // ----------------------------
        UserAddressNotFound = 7000,
        AddressNotFoundOrUnauthorized = 7001,
        DefaultAddressNotFound = 7002
    }
}

