using Monopoly.Core.Entities;
using Monopoly.Core.Interfaces;
using Monopoly.Core.Services;
using Moq;

namespace Monopoly.Tests.Core.Tests;

public class WarehouseServiceTests
{
    private readonly Mock<IDataProvider> _dataProviderMock;
    private readonly List<Pallet> _samplePallets;

    public WarehouseServiceTests()
    {
        _dataProviderMock = new Mock<IDataProvider>();
        _samplePallets = CreateSamplePallets();
        _dataProviderMock.Setup(dp => dp.GetPallets()).Returns(_samplePallets);
    }

    private List<Pallet> CreateSamplePallets()
    {
        var pallets = new List<Pallet>();
        for (var i = 0; i < 5; i++)
        {
            var pallet = new Pallet(100, 100, 100);
            var box = new Box(10, 10, 10, 5, DateOnly.FromDateTime(DateTime.Now.AddDays(i)));
            pallet.AddBox(box);
            pallets.Add(pallet);
        }

        return pallets;
    }

    [Fact]
    public void GetPalletsGroupedByExpirationDate_ShouldReturnGroupedPallets_ByExpirationDate()
    {
        // Arrange
        var service = new WarehouseService(_dataProviderMock.Object, _samplePallets);

        // Act
        var groupedPallets = service.GetPalletsGroupedByExpirationDate().ToList();

        // Assert
        Assert.Equal(5, groupedPallets.Count);
        foreach (var group in groupedPallets) Assert.True(group.All(p => p.ExpirationDate == group.Key));
    }

    [Fact]
    public void GetTopPalletsByBoxExpiration_ShouldReturnTopPallets_WithHighestBoxExpirationDate()
    {
        // Arrange
        var service = new WarehouseService(_dataProviderMock.Object, _samplePallets);

        // Act
        var topPallets = service.GetTopPalletsByBoxExpiration().ToList();

        // Assert
        Assert.Equal(3, topPallets.Count);
        var expectedTopPallets = _samplePallets
            .OrderByDescending(p => p.Boxes.Max(b => b.ExpirationDate))
            .ThenBy(p => p.CalculateVolume())
            .Take(3);

        Assert.Equal(expectedTopPallets, topPallets);
    }

    [Fact]
    public void GetTopPalletsByBoxExpiration_ShouldHandleEmptyPalletList()
    {
        // Arrange
        var emptyService = new WarehouseService(_dataProviderMock.Object, new List<Pallet>());

        // Act
        var topPallets = emptyService.GetTopPalletsByBoxExpiration();

        // Assert
        Assert.Empty(topPallets);
    }

    [Fact]
    public void GetPalletsGroupedByExpirationDate_ShouldExcludePallets_WithMaxExpirationDate()
    {
        // Arrange
        var palletWithMaxDate = new Pallet(100, 100, 100);
        palletWithMaxDate.AddBox(new Box(10, 10, 10, 5, DateOnly.MaxValue));
        _samplePallets.Add(palletWithMaxDate);

        var service = new WarehouseService(_dataProviderMock.Object, _samplePallets);

        // Act
        var groupedPallets = service.GetPalletsGroupedByExpirationDate().ToList();

        // Assert
        Assert.DoesNotContain(groupedPallets, g => g.Key == DateOnly.MaxValue);
    }
}