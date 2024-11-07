using Monopoly.Core.Interfaces;

namespace Monopoly.Core.Entities;

public class Box : IWarehouseObject
{
    public Box(double width, double height, double depth, double weight, DateOnly? expirationDate = null,
        DateOnly? productionDate = null)
    {
        if (width <= 0 || height <= 0 || depth <= 0 || weight <= 0)
            throw new ArgumentException("Размеры и вес коробки должны быть положительными числами.");

        Id = Guid.NewGuid();
        Width = width;
        Height = height;
        Depth = depth;
        Weight = weight;
        ExpirationDate = expirationDate ?? productionDate?.AddDays(100);
        ProductionDate = productionDate;
    }

    public DateOnly? ExpirationDate { get; private set; }
    public DateOnly? ProductionDate { get; private set; }
    public Guid Id { get; }
    public double Width { get; }
    public double Height { get; }
    public double Depth { get; }
    public double Weight { get; }

    public double CalculateVolume()
    {
        return Width * Height * Depth;
    }
}