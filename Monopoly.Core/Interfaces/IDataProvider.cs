using Monopoly.Core.Entities;

namespace Monopoly.Core.Interfaces;

public interface IDataProvider
{
    IEnumerable<Pallet> GetPallets();
}