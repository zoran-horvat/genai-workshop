namespace Web.Models;

public class MoneyBag
{
    private readonly Money[] _moneys;
    public IEnumerable<Money> Moneys => _moneys;

    public MoneyBag(Money money, params Money[] additional)
    {
        Money[] all = [money, .. additional];
        _moneys = all
            .GroupBy(m => m.Currency)
            .Select(g => g.Aggregate((m1, m2) => m1.AddStrict(m2)))
            .ToArray();
    }

    private MoneyBag(Money[] moneys) =>
        _moneys = moneys;

    public MoneyBag Add(Money money) =>
        _moneys.All(m => m.Currency != money.Currency) ? new([.._moneys, money])
        : money.Amount == 0 ? this
        : new(AddExisting(money).ToArray());

    private IEnumerable<Money> AddExisting(Money newMoney) =>
        _moneys.Select(m => m.Currency == newMoney.Currency ? m.AddStrict(newMoney) : m);

    public bool CanFold() =>
        _moneys.Length == 1;

    public Money Fold() =>
        _moneys.Length == 1 ? _moneys[0]
        : throw new InvalidOperationException("Cannot fold: MoneyBag contains multiple currencies.");
}