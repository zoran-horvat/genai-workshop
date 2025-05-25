using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneyPrecisionTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(0)]
    [InlineData(-2)]
    [InlineData(8)]
    public void ChangePrecision_Throws_IfNotAllowed(int newPrecision)
    {
        var money = new Money(2, 10.00m, new Currency("USD"));
        Assert.Throws<ArgumentException>(() => money.ChangePrecision(newPrecision));
    }

    [Theory]
    [InlineData(4, 2, 10.1299, 10.13)] // 4->2, rounds up
    [InlineData(4, 2, 10.1249, 10.12)] // 4->2, rounds down
    [InlineData(6, 4, 10.123499, 10.1235)] // 6->4, rounds up
    [InlineData(6, 4, 10.123444, 10.1234)] // 6->4, rounds down
    [InlineData(6, 2, 10.129999, 10.13)] // 6->2, rounds up
    [InlineData(6, 2, 10.124999, 10.12)] // 6->2, rounds down
    [InlineData(6, 2, 10.125000, 10.13)] // 6->2, midpoint rounds up
    [InlineData(6, 2, 10.120000, 10.12)] // 6->2, exact
    public void ChangePrecision_ReducesPrecisionAndRounds(int from, int to, decimal start, decimal expected)
    {
        var money = new Money(from, start, new Currency("USD"));
        var changed = money.ChangePrecision(to);
        Assert.Equal(to, changed.Precision);
        Assert.Equal(expected, changed.Amount);
        Assert.Equal(money.Currency, changed.Currency);
    }

    [Theory]
    [InlineData(2, 4, 10.12)]
    [InlineData(2, 6, 10.12)]
    [InlineData(4, 6, 10.1234)]
    public void ChangePrecision_IncreasesPrecisionAndRetainsAmount(int from, int to, decimal start)
    {
        var money = new Money(from, start, new Currency("USD"));
        var changed = money.ChangePrecision(to);
        Assert.Equal(to, changed.Precision);
        Assert.Equal(start, changed.Amount);
        Assert.Equal(money.Currency, changed.Currency);
    }
}