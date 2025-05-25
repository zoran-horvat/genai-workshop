using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneyConstructorTests
{
    [Theory]
    [InlineData(0, 2)]
    [InlineData(123.45, 2)]
    [InlineData(0.01, 2)]
    [InlineData(999999.99, 2)]
    [InlineData(1.2, 2)]
    [InlineData(1.0, 2)]
    [InlineData(0, 4)]
    [InlineData(123.4567, 4)]
    [InlineData(0.0001, 4)]
    [InlineData(999999.9999, 4)]
    [InlineData(1.23, 4)]
    [InlineData(1.2, 4)]
    [InlineData(1.0, 4)]
    [InlineData(0, 6)]
    [InlineData(123.456789, 6)]
    [InlineData(0.000001, 6)]
    [InlineData(999999.999999, 6)]
    [InlineData(1.23, 6)]
    [InlineData(1.2, 6)]
    [InlineData(1.0, 6)]
    public void Can_Initialize_Money_With_Allowed_Precision(decimal amount, int precision)
    {
        var currency = new Currency("USD");
        var money = new Money(precision, amount, currency);
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
        Assert.Equal(precision, money.Precision);
    }

    [Theory]
    [InlineData(-1.00, 2)]
    [InlineData(-0.01, 2)]
    [InlineData(-123.45, 2)]
    [InlineData(-1.0001, 4)]
    [InlineData(-123.4567, 4)]
    [InlineData(-1.000001, 6)]
    [InlineData(-123.456789, 6)]
    public void Rejects_Negative_Amounts(decimal amount, int precision)
    {
        var currency = new Currency("USD");
        Assert.Throws<ArgumentException>(() => new Money(precision, amount, currency));
    }

    [Theory]
    [InlineData(1.001, 2)]
    [InlineData(123.456, 2)]
    [InlineData(0.00001, 4)]
    [InlineData(123.45678, 4)]
    [InlineData(1.0000011, 6)]
    [InlineData(123.4567891, 6)]
    public void Rejects_Amounts_With_Too_Many_Decimals(decimal amount, int precision)
    {
        var currency = new Currency("USD");
        Assert.Throws<ArgumentException>(() => new Money(precision, amount, currency));
    }
}
