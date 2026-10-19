namespace ElectronicsStoreManagementSystem.V2._0;

internal class Cart
{
    private readonly List<CartItem> items = new();
    public IReadOnlyList<CartItem> Items => items.AsReadOnly();
    public decimal Total
    {
        get
        {
            decimal total = 0;
            foreach (CartItem item in items)
                total += item.Total;
            return total;
        }
    }

    public bool Add(Product? product, int quantity, out string error)
    {
        if (product is null) { error = "Товар не найден."; return false; }
        if (quantity < 1) { error = "Количество должно быть не меньше 1."; return false; }
        CartItem? existing = null;
        foreach (CartItem item in items)
        {
            if (item.Product.Id == product.Id)
            {
                existing = item;
                break;
            }
        }

        if (existing != null && existing.Product != product)
        {
            error = "Этот товар был удалён из каталога. Удалите старую позицию из корзины и добавьте товар заново.";
            return false;
        }

        int existingQuantity = existing == null ? 0 : existing.Quantity;
        if (existingQuantity > product.Quantity || quantity > product.Quantity - existingQuantity)
        {
            error = "В корзине будет больше товара, чем есть на складе.";
            return false;
        }

        int newQuantity = existingQuantity + quantity;
        if (existing is null) items.Add(new CartItem(product, newQuantity));
        else existing.SetQuantity(newQuantity);
        error = string.Empty;
        return true;
    }

    public bool SetQuantity(int productId, int quantity, out string error)
    {
        CartItem? item = null;
        foreach (CartItem cartItem in items)
        {
            if (cartItem.Product.Id == productId)
            {
                item = cartItem;
                break;
            }
        }

        if (item is null) { error = "Позиция не найдена в корзине."; return false; }
        if (quantity < 1) { error = "Количество должно быть не меньше 1."; return false; }
        if (quantity > item.Product.Quantity) { error = "Недостаточно товара на складе."; return false; }
        item.SetQuantity(quantity);
        error = string.Empty;
        return true;
    }

    public bool Remove(int productId)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Product.Id == productId)
            {
                items.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        items.Clear();
    }
}
