using Monopoly.Core.Interfaces;

namespace Monopoly.Core.Entities;

public class Pallet : IWarehouseObject
{
    public Pallet(double width, double height, double depth)
    {
        Id = Guid.NewGuid();
        Width = width;
        Height = height;
        Depth = depth;
        Boxes = new List<Box>();
    }

    public DateOnly ExpirationDate => Boxes.Any()
        ? Boxes.Min(box => box.ExpirationDate ?? DateOnly.MaxValue)
        : DateOnly.MaxValue;

    public List<Box> Boxes { get; set; }
    public Guid Id { get; }
    public double Width { get; }
    public double Height { get; }
    public double Depth { get; }
    public double Weight => Boxes.Sum(box => box.Weight) + 30;

    public double CalculateVolume()
    {
        var palletVolume = Width * Height * Depth;
        var boxesVolume = Boxes.Sum(box => box.CalculateVolume());
        return palletVolume + boxesVolume;
    }

    public void AddBox(Box box)
    {
        if (box.Width > Width || box.Depth > Depth)
            throw new InvalidOperationException("Размеры коробки превышают размеры паллеты.");

        Boxes.Add(box);
    }
}