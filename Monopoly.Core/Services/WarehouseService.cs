using Monopoly.Core.Entities;
using Monopoly.Core.Interfaces;

namespace Monopoly.Core.Services;

public class WarehouseService
{
    private readonly IDataProvider _dataProvider;
    private List<Pallet> _pallets;

    public WarehouseService(IDataProvider dataProvider, List<Pallet>? pallets = null)
    {
        _dataProvider = dataProvider;
        _pallets = pallets ?? _dataProvider.GetPallets().ToList();
    }

    private void LoadPallets()
    {
        _pallets = _dataProvider.GetPallets().ToList();
    }

    public IEnumerable<IGrouping<DateOnly, Pallet>> GetPalletsGroupedByExpirationDate()
    {
        return _pallets
            .Where(p => p.ExpirationDate != DateOnly.MaxValue)
            .OrderBy(p => p.ExpirationDate)
            .ThenBy(p => p.Weight)
            .GroupBy(p => p.ExpirationDate);
    }

    public IEnumerable<Pallet> GetTopPalletsByBoxExpiration(int topCount = 3)
    {
        return _pallets
            .Where(p => p.Boxes.Count != 0)
            .OrderByDescending(p => p.Boxes.Max(b => b.ExpirationDate))
            .ThenBy(p => p.CalculateVolume())
            .Take(topCount);
    }
}