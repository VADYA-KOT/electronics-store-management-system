namespace ElectronicsStoreManagementSystem.V2._0;

internal class Order
{
    public Order(int id, DateTime createdAt, IReadOnlyList<OrderItem> items)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "ID заказа должен быть больше нуля.");

        if (items == null)
            throw new ArgumentNullException(nameof(items), "Список позиций заказа не может быть null.");

        if (items.Count == 0)
            throw new ArgumentException("Заказ должен содержать хотя бы одну позицию.", nameof(items));

        foreach (OrderItem item in items)
        {
            if (item == null)
                throw new ArgumentException("Список позиций заказа не должен содержать null.", nameof(items));
        }

        Id = id;
        CreatedAt = createdAt;
        Items = Array.AsReadOnly(items.ToArray());

        decimal total = 0;
        foreach (OrderItem item in Items)
            total += item.Total;
        Total = total;
        Status = "Оформлен";
    }

    public int Id { get; }
    public DateTime CreatedAt { get; }
    public IReadOnlyList<OrderItem> Items { get; }
    public decimal Total { get; }
    public string Status { get; }
}
