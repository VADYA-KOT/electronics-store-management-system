namespace ElectronicsStoreManagementSystem.V2._0;

internal class Store
{
    private readonly List<Product> products = new();
    private readonly List<Order> orders = new();
    private int nextOrderId = 1;

    public IReadOnlyList<Product> Products => products.AsReadOnly();
    public IReadOnlyList<Order> Orders => orders.AsReadOnly();

    public bool AddProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (FindProductById(product.Id) is not null) return false;
        products.Add(product);
        return true;
    }

    public Product? FindProduct(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;

        foreach (Product product in products)
        {
            if (string.Equals(product.Name, name.Trim(), StringComparison.OrdinalIgnoreCase))
                return product;
        }

        return null;
    }

    public Product? FindProductById(int id)
    {
        foreach (Product product in products)
        {
            if (product.Id == id) return product;
        }

        return null;
    }

    public bool RemoveProduct(int id)
    {
        var product = FindProductById(id);
        return product is not null && products.Remove(product);
    }

    public bool ChangePrice(int id, decimal price)
    {
        var product = FindProductById(id);
        if (product is null || price <= 0) return false;
        product.SetPrice(price);
        return true;
    }

    public bool ChangeStock(int id, int quantity)
    {
        var product = FindProductById(id);
        if (product is null || quantity < 0) return false;
        product.SetQuantity(quantity);
        return true;
    }

    public Order? PlaceOrder(Customer customer, out string error)
    {
        ArgumentNullException.ThrowIfNull(customer);
        if (customer.Cart.Items.Count == 0)
        {
            error = "Корзина пуста.";
            return null;
        }

        foreach (var cartItem in customer.Cart.Items)
        {
            var current = FindProductById(cartItem.Product.Id);
            if (current is null || current != cartItem.Product)
            {
                error = $"Товар «{cartItem.Product.Name}» больше недоступен в каталоге. Удалите его из корзины и добавьте заново.";
                return null;
            }

            if (current.Quantity < cartItem.Quantity)
            {
                error = $"Недостаточно товара «{current.Name}» на складе. Заказ не оформлен.";
                return null;
            }
        }

        var items = new List<OrderItem>();
        foreach (var cartItem in customer.Cart.Items)
        {
            items.Add(new OrderItem(
                cartItem.Product.Id,
                cartItem.Product.Name,
                cartItem.Quantity,
                cartItem.Product.Price));
        }

        var order = new Order(nextOrderId, DateTime.Now, items);

        if (!customer.TryPay(order.Total))
        {
            error = "Недостаточно денег для оформления заказа.";
            return null;
        }

        nextOrderId++;

        foreach (var item in customer.Cart.Items)
            item.Product.ChangeQuantity(-item.Quantity);

        orders.Add(order);
        customer.AddOrder(order);
        customer.Cart.Clear();
        error = string.Empty;
        return order;
    }
}
