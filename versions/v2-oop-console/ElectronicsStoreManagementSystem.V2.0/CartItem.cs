namespace ElectronicsStoreManagementSystem.V2._0;

internal class CartItem
{
    public CartItem(Product product, int quantity)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product), "Товар не может быть null.");

        if (quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть не меньше 1.");

        Product = product;
        Quantity = quantity;
    }

    public Product Product { get; }
    public int Quantity { get; private set; }
    public decimal Total => Product.Price * Quantity;
    internal void SetQuantity(int quantity)
    {
        if (quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть не меньше 1.");

        Quantity = quantity;
    }
}
