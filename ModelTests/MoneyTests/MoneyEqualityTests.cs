using Web.Models;

namespace ModelTests.MoneyTests;

public class MoneyEqualityTests
{
    public static IEnumerable<object[]> GetEqualMonies()
    {
        yield return new object[] { new Money(2, 10.00m, new Currency("USD")), new Money(2, 10.00m, new Currency("USD")) };
        yield return new object[] { new Money(4, 10.1234m, new Currency("EUR")), new Money(4, 10.1234m, new Currency("EUR")) };
    }

    public static IEnumerable<object[]> GetUnequalMonies()
    {
        yield return new object[] { new Money(2, 10.00m, new Currency("USD")), new Money(2, 20.00m, new Currency("USD")) };
        yield return new object[] { new Money(2, 10.00m, new Currency("USD")), new Money(2, 10.00m, new Currency("EUR")) };
        yield return new object[] { new Money(2, 10.00m, new Currency("USD")), new Money(4, 10.0000m, new Currency("USD")) };
    }

    [Theory]
    [MemberData(nameof(GetEqualMonies))]
    public void Equals_ReturnsTrue_ForEqualMonies(Money a, Money b)
    {
        Assert.True(a.Equals(b));
    }

    [Theory]
    [MemberData(nameof(GetUnequalMonies))]
    public void Equals_ReturnsFalse_ForUnequalMonies(Money a, Money b)
    {
        Assert.False(a.Equals(b));
    }

    [Theory]
    [MemberData(nameof(GetEqualMonies))]
    public void OperatorEquals_ReturnsTrue_ForEqualMonies(Money a, Money b)
    {
        Assert.True(a == b);
    }

    [Theory]
    [MemberData(nameof(GetUnequalMonies))]
    public void OperatorEquals_ReturnsFalse_ForUnequalMonies(Money a, Money b)
    {
        Assert.False(a == b);
    }

    [Theory]
    [MemberData(nameof(GetEqualMonies))]
    public void OperatorNotEquals_ReturnsFalse_ForEqualMonies(Money a, Money b)
    {
        Assert.False(a != b);
    }

    [Theory]
    [MemberData(nameof(GetUnequalMonies))]
    public void OperatorNotEquals_ReturnsTrue_ForUnequalMonies(Money a, Money b)
    {
        Assert.True(a != b);
    }
}