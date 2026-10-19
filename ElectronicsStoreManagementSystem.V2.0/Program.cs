using ElectronicsStoreManagementSystem.V2._0;

var store = new Store();
store.AddProduct(new Product(1, "Ноутбук", 25000m, 5, "Ноутбуки"));
store.AddProduct(new Product(2, "Наушники", 2300m, 15, "Аудио"));
store.AddProduct(new Product(3, "Клавиатура", 3500m, 12, "Периферия"));
store.AddProduct(new Product(4, "Мышка", 2000m, 20, "Периферия"));
var customer = new Customer(1, "Покупатель", 10000m);

while (true)
{
    Console.WriteLine("\n================================");
    Console.WriteLine("        МАГАЗИН ЭЛЕКТРОНИКИ");
    Console.WriteLine("================================");
    Console.WriteLine("1. Режим администратора");
    Console.WriteLine("2. Режим покупателя");
    Console.WriteLine("0. Выход");
    switch (ReadInt("Выберите режим: "))
    {
        case 1: RunAdminMenu(store); break;
        case 2: RunCustomerMenu(store, customer); break;
        case 0: Console.WriteLine("До свидания!"); return;
        default: Console.WriteLine("Выберите пункт 0, 1 или 2."); break;
    }
}

static void RunAdminMenu(Store store)
{
    while (true)
    {
        Console.WriteLine("\n--- Администратор ---");
        Console.WriteLine("1. Каталог  2. Поиск  3. Добавить товар");
        Console.WriteLine("4. Изменить цену  5. Изменить остаток  6. Удалить товар");
        Console.WriteLine("7. Заказы  0. К выбору роли");
        switch (ReadInt("Действие: "))
        {
            case 1: ShowProducts(store); break;
            case 2: SearchProduct(store); break;
            case 3: AddProduct(store); break;
            case 4:
                var priceId = ReadInt("ID товара: ");
                var price = ReadDecimal("Новая цена: ");
                Console.WriteLine(store.ChangePrice(priceId, price) ? "Цена обновлена." : "Товар не найден или цена некорректна.");
                break;
            case 5:
                var stockId = ReadInt("ID товара: ");
                var stock = ReadInt("Новое количество на складе: ");
                Console.WriteLine(store.ChangeStock(stockId, stock) ? "Остаток обновлён." : "Товар не найден или количество некорректно.");
                break;
            case 6:
                Console.WriteLine(store.RemoveProduct(ReadInt("ID товара для удаления: ")) ? "Товар удалён." : "Товар не найден.");
                break;
            case 7: ShowOrders(store.Orders); break;
            case 0: return;
            default: Console.WriteLine("Неизвестный пункт меню."); break;
        }
    }
}

static void RunCustomerMenu(Store store, Customer customer)
{
    while (true)
    {
        Console.WriteLine($"\n--- Покупатель: {customer.Name} ---");
        Console.WriteLine("1. Каталог  2. Поиск  3. Добавить в корзину");
        Console.WriteLine("4. Корзина  5. Изменить количество  6. Удалить позицию");
        Console.WriteLine("7. Оформить заказ  8. Мои заказы  0. К выбору роли");
        switch (ReadInt("Действие: "))
        {
            case 1: ShowProducts(store); break;
            case 2: SearchProduct(store); break;
            case 3:
                ShowProducts(store);
                int productId = ReadInt("ID товара: ");
                Product? product = store.FindProductById(productId);
                if (product is null)
                {
                    Console.WriteLine("Товар не найден.");
                    break;
                }

                var quantity = ReadInt("Количество: ");
                Console.WriteLine(customer.Cart.Add(product, quantity, out var addError) ? "Товар добавлен в корзину." : addError);
                break;
            case 4: ShowCart(customer.Cart); break;
            case 5:
                var editId = ReadInt("ID товара: ");
                var newQuantity = ReadInt("Новое количество: ");
                Console.WriteLine(customer.Cart.SetQuantity(editId, newQuantity, out var editError) ? "Количество обновлено." : editError);
                break;
            case 6:
                Console.WriteLine(customer.Cart.Remove(ReadInt("ID товара: ")) ? "Позиция удалена." : "Позиция не найдена.");
                break;
            case 7:
                ShowCart(customer.Cart);
                if (customer.Cart.Items.Count == 0) break;
                if (ReadYesNo("Оформить заказ? (да/нет): "))
                {
                    var order = store.PlaceOrder(customer, out var orderError);
                    Console.WriteLine(order is null ? orderError : $"Заказ №{order.Id} оформлен на сумму {order.Total:N2}.");
                }
                break;
            case 8: ShowOrders(customer.Orders); break;
            case 0: return;
            default: Console.WriteLine("Неизвестный пункт меню."); break;
        }
    }
}

static void ShowProducts(Store store)
{
    Console.WriteLine("\nКаталог:");
    if (store.Products.Count == 0) { Console.WriteLine("Каталог пуст."); return; }
    foreach (var product in store.Products)
        Console.WriteLine($"ID: {product.Id} | {product.Name} | {product.Price:N2} | Остаток: {product.Quantity} | {product.Category}");
}

static void SearchProduct(Store store)
{
    Console.WriteLine("1. Поиск по ID  2. Поиск по названию");
    var choice = ReadInt("Способ поиска: ");
    Product? product = null;
    if (choice == 1)
        product = store.FindProductById(ReadInt("ID: "));
    else if (choice == 2)
        product = store.FindProduct(ReadText("Название: "));
    else
    {
        Console.WriteLine("Выберите 1 или 2.");
        return;
    }

    if (product is null) Console.WriteLine("Товар не найден.");
    else Console.WriteLine($"ID: {product.Id} | {product.Name} | {product.Price:N2} | Остаток: {product.Quantity} | {product.Category}");
}

static void AddProduct(Store store)
{
    try
    {
        var id = ReadInt("ID: ");
        var name = ReadText("Название: ");
        var price = ReadDecimal("Цена: ");
        var quantity = ReadInt("Количество: ");
        var category = ReadText("Категория: ");
        Console.WriteLine(store.AddProduct(new Product(id, name, price, quantity, category))
            ? "Товар добавлен."
            : "Товар с таким ID уже существует.");
    }
    catch (ArgumentException ex) { Console.WriteLine(ex.Message); }
    catch (OverflowException) { Console.WriteLine("Значение слишком велико."); }
}

static void ShowCart(Cart cart)
{
    Console.WriteLine("\nКорзина:");
    if (cart.Items.Count == 0) { Console.WriteLine("Корзина пуста."); return; }
    foreach (var item in cart.Items)
        Console.WriteLine($"ID: {item.Product.Id} | {item.Product.Name} | {item.Quantity} × {item.Product.Price:N2} = {item.Total:N2}");
    Console.WriteLine($"Итого: {cart.Total:N2}");
}

static void ShowOrders(IReadOnlyList<Order> orders)
{
    if (orders.Count == 0) { Console.WriteLine("Заказов пока нет."); return; }
    foreach (var order in orders)
    {
        Console.WriteLine($"\nЗаказ №{order.Id} от {order.CreatedAt:g} | {order.Status} | Итого: {order.Total:N2}");
        foreach (var item in order.Items)
            Console.WriteLine($"  {item.ProductName} (ID {item.ProductId}) — {item.Quantity} × {item.UnitPrice:N2} = {item.Total:N2}");
    }
}

static int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var value)) return value;
        Console.WriteLine("Введите целое число.");
    }
}

static decimal ReadDecimal(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (decimal.TryParse(Console.ReadLine(), out var value)) return value;
        Console.WriteLine("Введите число.");
    }
}

static string ReadText(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var value = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
        Console.WriteLine("Значение не может быть пустым.");
    }
}

static bool ReadYesNo(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? answer = Console.ReadLine();
        if (answer != null)
            answer = answer.Trim();

        bool isYes = string.Equals(answer, "д", StringComparison.OrdinalIgnoreCase)
            || string.Equals(answer, "да", StringComparison.OrdinalIgnoreCase);
        if (isYes) return true;

        bool isNo = string.Equals(answer, "н", StringComparison.OrdinalIgnoreCase)
            || string.Equals(answer, "нет", StringComparison.OrdinalIgnoreCase);
        if (isNo) return false;

        Console.WriteLine("Введите «д» или «н».");
    }
}
