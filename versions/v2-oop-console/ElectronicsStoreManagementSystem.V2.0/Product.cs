namespace ElectronicsStoreManagementSystem.V2._0;

internal class Product
{
    public Product(int id, string name, decimal price, int quantity, string category)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "ID должен быть больше нуля.");
        Id = id;
        SetName(name);
        SetPrice(price);
        SetQuantity(quantity);
        SetCategory(category);
    }

    public int Id { get; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public string Category { get; private set; } = string.Empty;

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Название не может быть пустым.", nameof(name));
        Name = name.Trim();
    }

    public void SetPrice(decimal price)
    {
        if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price), "Цена должна быть больше нуля.");
        Price = price;
    }

    public void SetQuantity(int quantity)
    {
        if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Количество не может быть отрицательным.");
        Quantity = quantity;
    }

    public void ChangeQuantity(int difference)
    {
        SetQuantity(checked(Quantity + difference));
    }

    public void SetCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Категория не может быть пустой.", nameof(category));
        Category = category.Trim();
    }
}
