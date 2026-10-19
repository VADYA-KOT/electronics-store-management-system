namespace ElectronicsStoreManagementSystem.V2._0;

internal class Customer
{
    private readonly List<Order> orders = new();

    public Customer(int id, string name, decimal money)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "ID покупателя должен быть больше нуля.");

        if (money < 0)
            throw new ArgumentOutOfRangeException(nameof(money), "Баланс не может быть отрицательным.");

        Id = id;
        Money = money;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя покупателя не может быть пустым.", nameof(name));

        Name = name.Trim();
    }

    public int Id { get; }
    public string Name { get; }
    public decimal Money { get; private set; }
    public Cart Cart { get; } = new();
    public IReadOnlyList<Order> Orders => orders.AsReadOnly();

    public bool TryPay(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сумма оплаты должна быть больше нуля.");

        if (amount > Money)
            return false;

        Money -= amount;
        return true;
    }

    internal void AddOrder(Order order)
    {
        orders.Add(order);
    }
}
