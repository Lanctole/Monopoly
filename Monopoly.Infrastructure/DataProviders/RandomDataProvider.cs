using Monopoly.Core.Entities;
using Monopoly.Core.Interfaces;

namespace Monopoly.Infrastructure.DataProviders;

public class RandomDataProvider : IDataProvider
{
    private readonly Random _random = new();

    public IEnumerable<Pallet> GetPallets()
    {
        var pallets = new List<Pallet>();

        const int minPalletCount = 10, maxPalletCount = 15;
        const int minBoxes = 0, maxBoxes = 8;

        const int minPalletSize = 50, maxPalletSize = 200;

        const int minBoxSize = 10, maxBoxSize = 50;
        const int minWeight = 1, maxWeight = 10;

        const int minDaysBeforeExpiration = -5, maxDaysBeforeExpiration = 10;
        const int maxDaysBeforeProduction = 10;

        var palletCount = _random.Next(minPalletCount, maxPalletCount);

        for (var i = 0; i < palletCount; i++)
        {
            var pallet = new Pallet(
                _random.Next(minPalletSize, maxPalletSize),
                _random.Next(minPalletSize, maxPalletSize),
                _random.Next(minPalletSize, maxPalletSize)
            );

            var boxCount = _random.Next(minBoxes, maxBoxes);
            for (var j = 0; j < boxCount; j++)
            {
                DateOnly? expirationDate = _random.Next(0, 2) == 0
                    ? DateOnly.FromDateTime(
                        DateTime.Now.AddDays(minDaysBeforeExpiration + _random.Next(1, maxDaysBeforeExpiration)))
                    : null;

                DateOnly? productionDate = expirationDate == null
                    ? DateOnly.FromDateTime(DateTime.Now.AddDays(-_random.Next(1, maxDaysBeforeProduction)))
                    : null;

                var box = new Box(
                    _random.Next(minBoxSize, maxBoxSize),
                    _random.Next(minBoxSize, maxBoxSize),
                    _random.Next(minBoxSize, maxBoxSize),
                    _random.Next(minWeight, maxWeight),
                    expirationDate,
                    productionDate
                );

                pallet.AddBox(box);
            }

            pallets.Add(pallet);
        }

        return pallets;
    }
}