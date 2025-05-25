using Web.Models;

namespace ModelTests.MoneyTests;

public class CurrencyTests
{
    [Fact]
    public void Currency_Symbol_CanBeSetAndRetrieved()
    {
        // Arrange
        var expectedSymbol = "USD";
        var currency = new Currency(expectedSymbol);

        // Act & Assert
        Assert.Equal(expectedSymbol, currency.Symbol);
    }

    [Theory]
    [InlineData("US")]
    [InlineData("USDD")]
    [InlineData("usd")]
    [InlineData("123")]
    [InlineData("")]
    public void Currency_Symbol_RejectsInvalidSymbols(string invalidSymbol)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Currency(invalidSymbol));
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("JPY")]
    [InlineData("GBP")]
    public void Currency_Symbol_AcceptsValidSymbols(string validSymbol)
    {
        // Act
        var currency = new Currency(validSymbol);

        // Assert
        Assert.Equal(validSymbol, currency.Symbol);
    }

    [Theory]
    [InlineData("USD", "USD", true)]
    [InlineData("USD", "EUR", false)]
    [InlineData("EUR", "EUR", true)]
    [InlineData("JPY", "GBP", false)]
    public void Currency_Equals_WorksAsExpected(string symbol1, string symbol2, bool expectedEqual)
    {
        var c1 = new Currency(symbol1);
        var c2 = new Currency(symbol2);

        Assert.Equal(expectedEqual, c1.Equals(c2));
        Assert.Equal(expectedEqual, c2.Equals(c1));
    }

    [Theory]
    [InlineData("USD", "USD", true)]
    [InlineData("USD", "EUR", false)]
    [InlineData("EUR", "EUR", true)]
    [InlineData("JPY", "GBP", false)]
    public void Currency_EqualityOperator_WorksAsExpected(string symbol1, string symbol2, bool expectedEqual)
    {
        var c1 = new Currency(symbol1);
        var c2 = new Currency(symbol2);

        Assert.Equal(expectedEqual, c1 == c2);
    }

    [Theory]
    [InlineData("USD", "USD", false)]
    [InlineData("USD", "EUR", true)]
    [InlineData("EUR", "EUR", false)]
    [InlineData("JPY", "GBP", true)]
    public void Currency_InequalityOperator_WorksAsExpected(string symbol1, string symbol2, bool expectedNotEqual)
    {
        var c1 = new Currency(symbol1);
        var c2 = new Currency(symbol2);

        Assert.Equal(expectedNotEqual, c1 != c2);
    }
}
