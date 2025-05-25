using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneyAnyAdditionTests
{
    [Fact]
    public void AddAny_Always_Returns_MoneyBag()
    {
        var a = new Money(2, 10.00m, new Currency("USD"));
        var b = new Money(2, 5.00m, new Currency("USD"));
        var result = a.AddAny(b);
        Assert.IsType<MoneyBag>(result);
    }

    [Fact]
    public void AddAny_SameCurrency_Returns_MoneyBag_With_One_Money()
    {
        var a = new Money(2, 10.00m, new Currency("USD"));
        var b = new Money(2, 5.00m, new Currency("USD"));
        var result = a.AddAny(b);
        Assert.Single(result.Moneys);
        var money = Assert.Single(result.Moneys);
        Assert.Equal("USD", money.Currency.Symbol);
        Assert.Equal(15.00m, money.Amount);
    }

    [Fact]
    public void AddAny_DifferentCurrencies_Returns_MoneyBag_With_Two_Moneys()
    {
        var a = new Money(2, 10.00m, new Currency("USD"));
        var b = new Money(2, 5.00m, new Currency("EUR"));
        var result = a.AddAny(b);
        Assert.Equal(2, result.Moneys.Count());
        Assert.Contains(a, result.Moneys);
        Assert.Contains(b, result.Moneys);
    }

    [Fact]
    public void AddAny_ThisIsZero_ReturnsOtherInBag()
    {
        var zero = new Money(2, 0m, new Currency("USD"));
        var other = new Money(2, 5.00m, new Currency("USD"));
        var result = zero.AddAny(other);
        var money = Assert.Single(result.Moneys);
        Assert.Equal(other, money);
    }

    [Fact]
    public void AddAny_OtherIsZero_ReturnsThisInBag()
    {
        var thisMoney = new Money(2, 10.00m, new Currency("USD"));
        var zero = new Money(2, 0m, new Currency("USD"));
        var result = thisMoney.AddAny(zero);
        var money = Assert.Single(result.Moneys);
        Assert.Equal(thisMoney, money);
    }
}