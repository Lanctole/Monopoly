namespace Monopoly.Core.Interfaces;

public interface IWarehouseObject
{
    Guid Id { get; }
    double Width { get; }
    double Height { get; }
    double Depth { get; }
    double Weight { get; }

    double CalculateVolume();
}