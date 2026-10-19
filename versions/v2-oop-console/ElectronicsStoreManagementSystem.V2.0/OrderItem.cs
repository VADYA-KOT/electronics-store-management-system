namespace ElectronicsStoreManagementSystem.V2._0;

internal class OrderItem
{
    public OrderItem(int productId, string productName, int quantity, decimal unitPrice)
    {
        if (productId <= 0)
            throw new ArgumentOutOfRangeException(nameof(productId), "ID товара должен быть больше нуля.");

        if (productName == null)
            throw new ArgumentNullException(nameof(productName), "Название товара не может быть null.");

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Название товара не может быть пустым.", nameof(productName));

        if (quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть не меньше 1.");

        if (unitPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Цена за единицу должна быть больше нуля.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public int ProductId { get; }
    public string ProductName { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    public decimal Total => UnitPrice * Quantity;
}
