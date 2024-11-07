using Monopoly.Core.Entities;

namespace Monopoly.Tests.Core.Tests;

public class BoxTests
{
    [Fact]
    public void CalculateVolume_ShouldReturnCorrectVolume()
    {
        // Arrange
        var box = new Box(50, 60, 70, 10);

        // Act
        var volume = box.CalculateVolume();

        // Assert
        var expectedVolume = 50 * 60 * 70;
        Assert.Equal(expectedVolume, volume);
    }

    [Fact]
    public void Box_ShouldHaveCorrectProperties_WhenCreated()
    {
        // Arrange
        var box = new Box(50, 60, 70, 10, DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            DateOnly.FromDateTime(DateTime.Now));

        // Act & Assert
        Assert.Equal(50, box.Width);
        Assert.Equal(60, box.Height);
        Assert.Equal(70, box.Depth);
        Assert.Equal(10, box.Weight);
        Assert.NotNull(box.ExpirationDate);
        Assert.NotNull(box.ProductionDate);
    }

    [Fact]
    public void ExpirationDate_ShouldBeNull_WhenNotProvided()
    {
        // Arrange
        var box = new Box(50, 60, 70, 10);

        // Act & Assert
        Assert.Null(box.ExpirationDate);
    }

    [Fact]
    public void ExpirationDate_ShouldReturnCorrectExpirationDate_WhenProvided()
    {
        // Arrange
        var box = new Box(50, 60, 70, 10, DateOnly.FromDateTime(DateTime.Now.AddDays(30)));

        // Act & Assert
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now.AddDays(30)), box.ExpirationDate);
    }

    [Fact]
    public void ProductionDate_ShouldReturnCorrectProductionDate_WhenProvided()
    {
        // Arrange
        var productionDate = DateOnly.FromDateTime(DateTime.Now);
        var box = new Box(50, 60, 70, 10, productionDate: productionDate);

        // Act & Assert
        Assert.Equal(productionDate, box.ProductionDate);
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenCreatingBoxWithNegativeDimensions()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Box(-50, 60, 70, 10));
        Assert.Throws<ArgumentException>(() => new Box(50, -60, 70, 10));
        Assert.Throws<ArgumentException>(() => new Box(50, 60, -70, 10));
        Assert.Throws<ArgumentException>(() => new Box(50, 60, 70, -10));
    }
}