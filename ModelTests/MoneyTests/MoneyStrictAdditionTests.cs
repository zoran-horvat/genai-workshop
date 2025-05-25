using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneyStrictAdditionTests
{
    public static IEnumerable<object[]> GetValidAdditions()
    {
        yield return new object[] {
            new Money(2, 10.00m, new Currency("USD")),
            new Money(2, 5.00m, new Currency("USD")),
            new Money(2, 15.00m, new Currency("USD"))
        };
        yield return new object[] {
            new Money(4, 1.1234m, new Currency("EUR")),
            new Money(4, 2.8766m, new Currency("EUR")),
            new Money(4, 4.0000m, new Currency("EUR"))
        };
        // Different precisions, result should have greatest precision
        yield return new object[] {
            new Money(2, 1.23m, new Currency("USD")),
            new Money(4, 2.3456m, new Currency("USD")),
            new Money(4, 3.5756m, new Currency("USD"))
        };
        yield return new object[] {
            new Money(6, 0.000001m, new Currency("USD")),
            new Money(2, 1.00m, new Currency("USD")),
            new Money(6, 1.000001m, new Currency("USD"))
        };
    }

    public static IEnumerable<object[]> GetInvalidAdditions()
    {
        // Different currency
        yield return new object[] {
            new Money(2, 10.00m, new Currency("USD")),
            new Money(2, 5.00m, new Currency("EUR"))
        };
    }

    [Theory]
    [MemberData(nameof(GetValidAdditions))]
    public void AddStrict_ReturnsSum_WhenCurrencyMatches(Money a, Money b, Money expected)
    {
        var result = a.AddStrict(b);
        Assert.Equal(expected.Amount, result.Amount);
        Assert.Equal(expected.Currency, result.Currency);
        Assert.Equal(expected.Precision, result.Precision);
    }

    [Theory]
    [MemberData(nameof(GetInvalidAdditions))]
    public void AddStrict_Throws_WhenCurrencyDiffers(Money a, Money b)
    {
        Assert.Throws<ArgumentException>(() => a.AddStrict(b));
    }
}