using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneySubtractionTests
{
    [Fact]
    public void SubtractStrict_Throws_When_Currencies_Differ()
    {
        var usd = new Currency("USD");
        var eur = new Currency("EUR");
        var m1 = new Money(2, 10.00m, usd);
        var m2 = new Money(2, 5.00m, eur);
        Assert.Throws<ArgumentException>(() => m1.SubtractStrict(m2));
    }

    [Fact]
    public void SubtractStrict_ResultPrecision_Is_Higher_Of_Two()
    {
        var usd = new Currency("USD");
        var m1 = new Money(2, 10.00m, usd);
        var m2 = new Money(4, 1.1234m, usd);
        // Expected precision is 4
        // 10.0000 - 1.1234 = 8.8766
        var result = m1.ChangePrecision(4).SubtractStrict(m2);
        Assert.Equal(4, result.Precision);
        Assert.Equal(8.8766m, result.Amount);
    }

    [Fact]
    public void SubtractStrict_Throws_When_Result_Would_Be_Negative()
    {
        var usd = new Currency("USD");
        var m1 = new Money(2, 5.00m, usd);
        var m2 = new Money(2, 10.00m, usd);
        Assert.Throws<ArgumentException>(() => m1.SubtractStrict(m2));
    }

    [Fact]
    public void SubtractStrict_ResultIsZero_When_AmountsAreEqual()
    {
        var usd = new Currency("USD");
        var m1 = new Money(2, 5.00m, usd);
        var m2 = new Money(2, 5.00m, usd);
        var result = m1.SubtractStrict(m2);
        Assert.Equal(2, result.Precision);
        Assert.Equal(0.00m, result.Amount);
        Assert.Equal(usd, result.Currency);
    }
}