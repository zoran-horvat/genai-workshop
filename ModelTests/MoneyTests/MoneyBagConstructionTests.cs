using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneyBagConstructionTests
{
    [Fact]
    public void Add_Does_Not_Mutate_Existing_Instance()
    {
        // Arrange
        var money = new Money(2, 10, new Currency("USD"));
        var bag = new MoneyBag(money);

        // Act
        var newMoney = new Money(2, 5, new Currency("EUR"));
        var newBag = bag.Add(newMoney);

        // Assert
        Assert.Single(bag.Moneys); // original bag remains unchanged
        Assert.NotSame(bag, newBag); // newBag is a different instance
        Assert.Contains(money, bag.Moneys);
        Assert.Contains(newMoney, newBag.Moneys);
    }

    [Fact]
    public void MoneyBag_Requires_At_Least_One_Money()
    {
        // Arrange
        var usd = new Currency("USD");
        var money = new Money(2, 10, usd);

        // Act
        var bag = new MoneyBag(money);

        // Assert
        Assert.Single(bag.Moneys);
        Assert.Contains(money, bag.Moneys);
    }

    [Fact]
    public void MoneyBag_Can_Be_Instantiated_With_Multiple_Moneys()
    {
        // Arrange
        var usd = new Currency("USD");
        var eur = new Currency("EUR");
        var m1 = new Money(2, 10, usd);
        var m2 = new Money(2, 20, eur);

        // Act
        var bag = new MoneyBag(m1, m2);

        // Assert
        Assert.Equal(2, bag.Moneys.Count());
        Assert.Contains(m1, bag.Moneys);
        Assert.Contains(m2, bag.Moneys);
    }

    [Fact]
    public void MoneyBag_With_Two_Same_Currency_Moneys_Is_Summed()
    {
        // Arrange
        var usd = new Currency("USD");
        var m1 = new Money(2, 10, usd);
        var m2 = new Money(2, 20, usd);

        // Act
        var bag = new MoneyBag(m1, m2);

        // Assert
        Assert.Single(bag.Moneys);
        var expected = m1.AddStrict(m2);
        Assert.Equal(expected, bag.Moneys.Single());
    }
}