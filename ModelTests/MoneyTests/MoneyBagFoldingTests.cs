using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneyBagFoldingTests
{
    [Fact]
    public void CanFold_ReturnsTrue_WhenSingleCurrency()
    {
        // Arrange
        var money = new Money(2, 100m, new Currency("USD"));
        var bag = new MoneyBag(money);

        // Act
        var result = bag.CanFold();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanFold_ReturnsFalse_WhenMultipleCurrencies()
    {
        // Arrange
        var usd = new Money(2, 100m, new Currency("USD"));
        var eur = new Money(2, 50m, new Currency("EUR"));
        var bag = new MoneyBag(usd, eur);

        // Act
        var result = bag.CanFold();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Fold_ReturnsMoney_WhenSingleCurrency()
    {
        // Arrange
        var money = new Money(2, 100m, new Currency("USD"));
        var bag = new MoneyBag(money);

        // Act
        var result = bag.Fold();

        // Assert
        Assert.Equal(money, result);
    }

    [Fact]
    public void Fold_ThrowsException_WhenMultipleCurrencies()
    {
        // Arrange
        var usd = new Money(2, 100m, new Currency("USD"));
        var eur = new Money(2, 50m, new Currency("EUR"));
        var bag = new MoneyBag(usd, eur);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => bag.Fold());
    }
}