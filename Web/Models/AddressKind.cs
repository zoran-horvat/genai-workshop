namespace Web.Models;

[Flags]
public enum AddressKind
{
    Headquarter = 1 << 0,
    Billing = 1 << 1,
    Branch = 1 << 2,
    Legal = 1 << 3,
    Other = 1 << 4,
    Default = Headquarter | Billing | Legal
}

public static class AddressKindExtensions
{
    public static IEnumerable<AddressKind> ToFlags(this AddressKind kind) =>
        Enum.GetValues(typeof(AddressKind))
            .Cast<AddressKind>()
            .Where(k => k != AddressKind.Default && k != 0 && kind.HasFlag(k));
}