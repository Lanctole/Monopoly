using Microsoft.Extensions.DependencyInjection;
using Monopoly.Core.Entities;
using Monopoly.Core.Interfaces;
using Monopoly.Core.Services;
using Monopoly.Infrastructure.DataProviders;

namespace Monopoly.UI.ConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IDataProvider, RandomDataProvider>()
            .AddSingleton<List<Pallet>>(provider => null)
            .AddSingleton<WarehouseService>()
            .BuildServiceProvider();

        var warehouseService = serviceProvider.GetService<WarehouseService>();

        Console.WriteLine("Добро пожаловать!");
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("1 - Показать все паллеты, сгруппированные по сроку годности");
        Console.WriteLine("2 - Показать топ-3 паллеты по максимальному сроку годности коробок");
        Console.WriteLine("3 - Выход");

        while (true)
        {
            Console.Write("\nВведите команду: ");
            var command = Console.ReadLine();

            switch (command)
            {
                case "1":
                    DisplayPalletsByExpiration(warehouseService);
                    break;
                case "2":
                    DisplayTopPallets(warehouseService);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Неверная команда. Попробуйте снова.");
                    break;
            }
        }
    }

    private static void DisplayPalletsByExpiration(WarehouseService warehouseService)
    {
        Console.WriteLine("\nПаллеты, сгруппированные по сроку годности:");
        var groupedPallets = warehouseService.GetPalletsGroupedByExpirationDate();

        foreach (var group in groupedPallets)
        {
            Console.WriteLine($"Срок годности: {group.Key}");

            foreach (var pallet in group)
            {
                Console.WriteLine(
                    $"Паллета ID: {pallet.Id}, Вес: {pallet.Weight} кг, Объем: {pallet.CalculateVolume()} куб. см");

                foreach (var box in pallet.Boxes)
                    Console.WriteLine(
                        $"   - Коробка ID: {box.Id}, Срок годности: {box.ExpirationDate}, Вес: {box.Weight} кг, Объем: {box.CalculateVolume()} куб. см");
            }

            Console.WriteLine();
        }
    }

    private static void DisplayTopPallets(WarehouseService warehouseService)
    {
        Console.WriteLine("\nТоп-3 паллеты с наибольшим сроком годности коробок:");
        var topPallets = warehouseService.GetTopPalletsByBoxExpiration();

        foreach (var pallet in topPallets)
        {
            Console.WriteLine(
                $"Паллета ID: {pallet.Id}, Максимальный срок годности коробки в паллете: {pallet.Boxes.Max(b => b.ExpirationDate)}, Вес: {pallet.Weight} кг, Объем: {pallet.CalculateVolume()} куб. см");
            foreach (var box in pallet.Boxes)
                Console.WriteLine(
                    $"   - Коробка: ID: {box.Id}, Срок годности: {box.ExpirationDate}, Вес: {box.Weight} кг, Объем: {box.CalculateVolume()} куб. см");

            Console.WriteLine();
        }
    }
}