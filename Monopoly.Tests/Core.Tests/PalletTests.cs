using Monopoly.Core.Entities;

namespace Monopoly.Tests.Core.Tests;

public class PalletTests
{
    [Fact]
    public void CalculateVolume_ShouldReturnCorrectVolume_WhenBoxesAreAdded()
    {
        // Arrange
        var pallet = new Pallet(100, 200, 300);
        var box1 = new Box(50, 60, 70, 10);
        var box2 = new Box(30, 40, 50, 5);

        // Act
        pallet.AddBox(box1);
        pallet.AddBox(box2);
        var totalVolume = pallet.CalculateVolume();

        // Assert
        var expectedVolume = 100 * 200 * 300 + 50 * 60 * 70 + 30 * 40 * 50;
        Assert.Equal(expectedVolume, totalVolume);
    }

    [Fact]
    public void AddBox_ShouldThrowInvalidOperationException_WhenBoxExceedsPalletSize()
    {
        // Arrange
        var pallet = new Pallet(100, 200, 300);
        var box = new Box(150, 200, 300, 10);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => pallet.AddBox(box));
    }

    [Fact]
    public void AddBox_ShouldIncreaseBoxCount_WhenBoxIsAdded()
    {
        // Arrange
        var pallet = new Pallet(100, 200, 300);
        var box = new Box(50, 60, 70, 10);

        // Act
        pallet.AddBox(box);

        // Assert
        Assert.Single(pallet.Boxes);
    }

    [Fact]
    public void Weight_ShouldReturnCorrectTotalWeight_WhenBoxesAreAdded()
    {
        // Arrange
        var pallet = new Pallet(100, 200, 300);
        var box1 = new Box(50, 60, 70, 10);
        var box2 = new Box(30, 40, 50, 5);

        // Act
        pallet.AddBox(box1);
        pallet.AddBox(box2);

        // Assert
        var expectedWeight = 30 + 10 + 5;
        Assert.Equal(expectedWeight, pallet.Weight);
    }

    [Fact]
    public void ExpirationDate_ShouldReturnEarliestExpirationDate_WhenBoxesAreAdded()
    {
        // Arrange
        var pallet = new Pallet(100, 200, 300);
        var box1 = new Box(50, 60, 70, 10, DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
            DateOnly.FromDateTime(DateTime.Now));
        var box2 = new Box(30, 40, 50, 5, DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
            DateOnly.FromDateTime(DateTime.Now));

        // Act
        pallet.AddBox(box1);
        pallet.AddBox(box2);

        // Assert
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now.AddDays(5)), pallet.ExpirationDate);
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenAddingBoxWithNegativeDimensions()
    {
        // Arrange
        var pallet = new Pallet(100, 200, 300);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => pallet.AddBox(new Box(-50, 60, 70, 10)));
        Assert.Throws<ArgumentException>(() => pallet.AddBox(new Box(50, -60, 70, 10)));
        Assert.Throws<ArgumentException>(() => pallet.AddBox(new Box(50, 60, -70, 10)));
        Assert.Throws<ArgumentException>(() => pallet.AddBox(new Box(50, 60, 70, -10)));
    }
}