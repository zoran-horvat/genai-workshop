using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneyBagAddingTests
{
    [Fact]
    public void Add_Money_With_Existing_Currency_Sums_Amounts()
    {
        // Arrange
        var bag = new MoneyBag(new Money(2, 2.50m, new Currency("USD")));
        var addMoney = new Money(2, 3.75m, new Currency("USD"));

        // Act
        var newBag = bag.Add(addMoney);

        // Assert
        Assert.Single(newBag.Moneys);
        var result = newBag.Moneys.First();
        Assert.Equal("USD", result.Currency.Symbol);
        Assert.Equal(6.25m, result.Amount); // 2.50 + 3.75 = 6.25
    }

    [Fact]
    public void Add_Money_With_New_Currency_Adds_New_Entry()
    {
        // Arrange
        var bag = new MoneyBag(new Money(2, 2.50m, new Currency("USD")));
        var addMoney = new Money(2, 1.20m, new Currency("EUR"));

        // Act
        var newBag = bag.Add(addMoney);

        // Assert
        Assert.Equal(2, newBag.Moneys.Count());
        Assert.Contains(newBag.Moneys, m => m.Currency.Symbol == "USD");
        Assert.Contains(newBag.Moneys, m => m.Currency.Symbol == "EUR");
    }

    [Fact]
    public void Add_Zero_Money_Returns_Same_Instance()
    {
        // Arrange
        var bag = new MoneyBag(new Money(2, 2.50m, new Currency("USD")));
        var zeroMoney = new Money(2, 0m, new Currency("USD"));

        // Act
        var newBag = bag.Add(zeroMoney);

        // Assert
        Assert.Same(bag, newBag);
    }

    [Fact]
    public void Add_Money_With_Cents_Overflow_Adjusts_Precision()
    {
        // Arrange
        var bag = new MoneyBag(new Money(2, 1.90m, new Currency("USD")));
        var addMoney = new Money(4, 0.2011m, new Currency("USD"));

        // Act
        var newBag = bag.Add(addMoney);

        // Assert
        var result = newBag.Moneys.First(m => m.Currency.Symbol == "USD");
        Assert.Equal(2.1011m, result.Amount); // 1.90 + 0.2011 = 2.1011 (precision 4)
        Assert.Equal(4, result.Precision); // Result should have the greater precision
    }
}