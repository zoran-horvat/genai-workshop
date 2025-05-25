namespace Web.Models;

public class Money
{
    public int Precision { get; }
    public Currency Currency { get; }
    public decimal Amount { get; }

    public Money(int precision, decimal amount, Currency currency)
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));
        if (!IsValidPrecision(precision)) throw new ArgumentException("Only precisions 2, 4, and 6 are allowed", nameof(precision));
        if (DecimalPlaces(amount) > precision) throw new ArgumentException($"Amount has more decimals than allowed precision {precision}", nameof(amount));
        
        Amount = amount;
        Currency = currency;
        Precision = precision;
    }

    private static bool IsValidPrecision(int precision) => precision == 2 || precision == 4 || precision == 6;

    private static int DecimalPlaces(decimal value)
    {
        value = Math.Abs(value);
        int[] bits = decimal.GetBits(value);
        int exponent = (bits[3] >> 16) & 0xFF;
        return exponent;
    }

    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) ? true
        : obj is Money other &&
            this.Precision == other.Precision &&
            this.Amount == other.Amount &&
            this.Currency == other.Currency;

    public override int GetHashCode() =>
        HashCode.Combine(Precision, Amount, Currency);

    public static bool operator ==(Money? left, Money? right) =>
        ReferenceEquals(left, right) ? true
        : left is not null && right is not null && left.Equals(right);

    public static bool operator !=(Money? left, Money? right) =>
        !(left == right);

    public MoneyBag AddAny(Money other) =>
        new MoneyBag(this, other);

    public Money AddStrict(Money other)
    {
        if (this.Currency != other.Currency)
            throw new ArgumentException("Cannot add Money with different currency.", nameof(other));
        int resultPrecision = Math.Max(this.Precision, other.Precision);
        var a = this.ChangePrecision(resultPrecision);
        var b = other.ChangePrecision(resultPrecision);
        decimal sum = a.Amount + b.Amount;
        return new Money(resultPrecision, sum, this.Currency);
    }

    public Money SubtractStrict(Money other)
    {
        if (this.Currency != other.Currency) throw new ArgumentException("Cannot subtract Money with different currency.", nameof(other));
        if (this.Amount < other.Amount) throw new ArgumentException("Cannot subtract a larger amount from a smaller one.", nameof(other));

        int resultPrecision = Math.Max(this.Precision, other.Precision);
        var a = this.ChangePrecision(resultPrecision);
        var b = other.ChangePrecision(resultPrecision);

        return new Money(resultPrecision, a.Amount - b.Amount, this.Currency);
    }

    public Money Multiply(decimal factor)
    {
        decimal newAmount = Math.Round(Amount * factor, Precision, MidpointRounding.AwayFromZero);
        return new Money(Precision, newAmount, Currency);
    }

    public Money ChangePrecision(int newPrecision)
    {
        if (newPrecision == Precision) return this;
        if (!IsValidPrecision(newPrecision)) throw new ArgumentException("Only precisions 2, 4, and 6 are allowed", nameof(newPrecision));

        if (newPrecision > Precision) return new Money(newPrecision, Amount, Currency);

        decimal rounded = Math.Round(Amount, newPrecision, MidpointRounding.AwayFromZero);
        return new Money(newPrecision, rounded, Currency);
    }
}