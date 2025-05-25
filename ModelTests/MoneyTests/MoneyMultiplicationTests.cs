using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneyMultiplicationTests
{
    [Fact]
    public void Multiply_ResultHasSamePrecisionAndCurrency()
    {
        var usd = new Currency("USD");
        var m = new Money(2, 10.25m, usd);
        var result = m.Multiply(2);
        Assert.Equal(2, result.Precision);
        Assert.Equal(usd, result.Currency);
    }

    [Fact]
    public void Multiply_ResultIsRoundedToPrecision()
    {
        var usd = new Currency("USD");
        var m = new Money(2, 10.25m, usd);
        // 10.25 * 2.333 = 23.91325 -> rounded to 23.91
        var result = m.Multiply(2.333m);
        Assert.Equal(23.91m, result.Amount);
    }

    [Fact]
    public void Multiply_ByZero_ReturnsZeroAmount()
    {
        var usd = new Currency("USD");
        var m = new Money(2, 10.25m, usd);
        var result = m.Multiply(0);
        Assert.Equal(0.00m, result.Amount);
        Assert.Equal(2, result.Precision);
    }
}