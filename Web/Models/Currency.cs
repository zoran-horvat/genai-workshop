namespace Web.Models;

public record Currency
{
    public string Symbol { get; }

    public Currency(string symbol)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(symbol, "^[A-Z]{3}$"))
            throw new ArgumentException("Symbol must be a valid ISO 4217 code (3 uppercase letters)", nameof(symbol));
        Symbol = symbol;
    }
}