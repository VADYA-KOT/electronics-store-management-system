Dictionary<string, double> products = new Dictionary<string, double>()
{
    { "Ноутбук", 85000 }, { "Телефон", 60000 }, { "Наушники", 8000 },
    { "Монитор", 25000 }, { "Клавиатура", 5000 }, { "Мышь", 3000 }
};

int mainChoice;
do
{
    Console.WriteLine("========== МАГАЗИН ЭЛЕКТРОНИКИ ==========");
    Console.WriteLine("1. Показать ассортимент\n2. Найти товар\n3. Добавить товар");
    Console.WriteLine("4. Изменить цену\n5. Удалить товар\n6. Фильтрация товаров");
    Console.WriteLine("7. Сортировка товаров\n8. Статистика\n9. Управление покупателями");
    Console.WriteLine("10. Создать заказ\n0. Выход");

    if (!int.TryParse(Console.ReadLine(), out mainChoice))
    {
        Console.WriteLine("Введите номер пункта меню.");
        mainChoice = -1;
        continue;
    }

    switch (mainChoice)
    {
        case 1:
            Console.WriteLine("========== АССОРТИМЕНТ ==========");
            if (products.Count == 0)
            {
                Console.WriteLine("Каталог пуст.");
            }
            else
            {
                foreach (var product in products)
                {
                    Console.WriteLine(product.Key + "\nЦена: " + product.Value);
                }
            }

            Console.WriteLine("0. Назад");
            int assortmentChoice;
            do
            {
                int.TryParse(Console.ReadLine(), out assortmentChoice);
            } while (assortmentChoice != 0);
            break;

        case 2:
            Console.WriteLine("Введите название товара");
            string nameSearchProduct = Console.ReadLine() ?? string.Empty;
            if (products.ContainsKey(nameSearchProduct))
            {
                Console.WriteLine(nameSearchProduct + "\nЦена: " + products[nameSearchProduct]);
            }
            else
            {
                Console.WriteLine("Данного товара нет.");
            }
            break;

        case 3:
            Console.WriteLine("Введите название товара, который хотите добавить");
            string nameAddProduct = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nameAddProduct))
            {
                Console.WriteLine("Название товара не может быть пустым.");
            }
            else if (products.ContainsKey(nameAddProduct))
            {
                Console.WriteLine("Товар с таким названием уже существует.");
            }
            else
            {
                Console.WriteLine("Введите цену нового товара");
                if (double.TryParse(Console.ReadLine(), out double priceNewProduct) && priceNewProduct > 0)
                {
                    products.Add(nameAddProduct, priceNewProduct);
                    Console.WriteLine("Товар успешно добавлен.");
                }
                else
                {
                    Console.WriteLine("Введите корректную цену больше нуля.");
                }
            }
            break;

        case 4:
            Console.WriteLine("Введите название товара, цену которого хотите изменить");
            string changePriceProduct = Console.ReadLine() ?? string.Empty;
            if (!products.ContainsKey(changePriceProduct))
            {
                Console.WriteLine("Данного товара не существует.");
            }
            else
            {
                Console.WriteLine("Введите новую цену товара");
                if (double.TryParse(Console.ReadLine(), out double newPriceOldProduct) && newPriceOldProduct > 0)
                {
                    products[changePriceProduct] = newPriceOldProduct;
                    Console.WriteLine("Цена успешно изменена.");
                }
                else
                {
                    Console.WriteLine("Введите корректную цену больше нуля.");
                }
            }
            break;

        case 5:
            Console.WriteLine("Введите название товара, который хотите удалить");
            string deleteProduct = Console.ReadLine() ?? string.Empty;
            if (products.Remove(deleteProduct))
            {
                Console.WriteLine("Товар успешно удалён.");
            }
            else
            {
                Console.WriteLine("Данного товара не существует.");
            }
            break;

        case 6:
            Console.WriteLine("Выберите фильтр цен");
            Console.WriteLine("1. Товары дешевле указанной цены\n2. Товары дороже указанной цены");
            Console.WriteLine("3. Товары в диапазоне цен\n0. Назад");
            int.TryParse(Console.ReadLine(), out int filterChoice);

            switch (filterChoice)
            {
                case 1:
                    Console.WriteLine("Укажите цену, до которой будут отображаться товары");
                    if (double.TryParse(Console.ReadLine(), out double priceBelow) && priceBelow > 0)
                    {
                        foreach (var product in products.Where(product => product.Value < priceBelow).OrderByDescending(product => product.Value))
                        {
                            Console.WriteLine(product.Key + "\nЦена: " + product.Value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Введите корректную цену больше нуля.");
                    }
                    break;

                case 2:
                    Console.WriteLine("Укажите цену, от которой будут отображаться товары");
                    if (double.TryParse(Console.ReadLine(), out double priceMore) && priceMore > 0)
                    {
                        foreach (var product in products.Where(product => product.Value > priceMore).OrderByDescending(product => product.Value))
                        {
                            Console.WriteLine(product.Key + "\nЦена: " + product.Value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Введите корректную цену больше нуля.");
                    }
                    break;

                case 3:
                    Console.WriteLine("Укажите минимальную цену");
                    bool isMinPriceCorrect = double.TryParse(Console.ReadLine(), out double minPrice) && minPrice > 0;
                    Console.WriteLine("Укажите максимальную цену");
                    bool isMaxPriceCorrect = double.TryParse(Console.ReadLine(), out double maxPrice) && maxPrice > 0;

                    if (!isMinPriceCorrect || !isMaxPriceCorrect)
                    {
                        Console.WriteLine("Введите корректные цены больше нуля.");
                    }
                    else if (minPrice > maxPrice)
                    {
                        Console.WriteLine("Минимальная цена не может быть больше максимальной.");
                    }
                    else
                    {
                        foreach (var product in products.Where(product => product.Value >= minPrice && product.Value <= maxPrice).OrderByDescending(product => product.Value))
                        {
                            Console.WriteLine(product.Key + "\nЦена: " + product.Value);
                        }
                    }
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine("Такого фильтра нет.");
                    break;
            }
            break;

        case 7:
            Console.WriteLine("Выберите тип сортировки товара");
            Console.WriteLine("1. От дешёвых к дорогим\n2. От дорогих к дешёвым");
            Console.WriteLine("3. По названию А–Я\n4. По названию Я–А\n0. Назад");
            int.TryParse(Console.ReadLine(), out int sortChoice);

            switch (sortChoice)
            {
                case 1:
                    foreach (var product in products.OrderBy(product => product.Value))
                    {
                        Console.WriteLine(product.Key + "\nЦена: " + product.Value);
                    }
                    break;

                case 2:
                    foreach (var product in products.OrderByDescending(product => product.Value))
                    {
                        Console.WriteLine(product.Key + "\nЦена: " + product.Value);
                    }
                    break;

                case 3:
                    foreach (var product in products.OrderBy(product => product.Key))
                    {
                        Console.WriteLine(product.Key + "\nЦена: " + product.Value);
                    }
                    break;

                case 4:
                    foreach (var product in products.OrderByDescending(product => product.Key))
                    {
                        Console.WriteLine(product.Key + "\nЦена: " + product.Value);
                    }
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine("Такого варианта сортировки нет.");
                    break;
            }
            break;

        case 8:
            Console.WriteLine("========== СТАТИСТИКА ТОВАРОВ ==========");
            Console.WriteLine("1. Общая статистика\n2. Анализ цен\n3. Анализ товаров\n0. Назад");
            int.TryParse(Console.ReadLine(), out int statsChoice);

            switch (statsChoice)
            {
                case 1:
                    if (products.Count == 0)
                    {
                        Console.WriteLine("Невозможно показать статистику: каталог пуст.");
                    }
                    else
                    {
                        Console.WriteLine("Количество товаров: " + products.Count);
                        Console.WriteLine("Средняя цена товаров: " + products.Average(product => product.Value));
                        var minPriceProduct = products.MinBy(product => product.Value);
                        Console.WriteLine("Самый дешёвый товар: " + minPriceProduct.Key + " — " + minPriceProduct.Value + " руб.");
                        var maxPriceProduct = products.MaxBy(product => product.Value);
                        Console.WriteLine("Самый дорогой товар: " + maxPriceProduct.Key + " — " + maxPriceProduct.Value + " руб.");
                    }
                    break;

                case 2:
                    Console.WriteLine("Анализ цен пока не реализован.");
                    break;

                case 3:
                    Console.WriteLine("Анализ товаров пока не реализован.");
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine("Такого раздела статистики нет.");
                    break;
            }
            break;

        case 9:
            Console.WriteLine("Управление покупателями пока не реализовано.");
            break;

        case 10:
            Console.WriteLine("Создание заказа пока не реализовано.");
            break;

        case 0:
            Console.WriteLine("Работа программы завершена.");
            break;

        default:
            Console.WriteLine("Такого пункта меню нет.");
            break;
    }
} while (mainChoice != 0);
