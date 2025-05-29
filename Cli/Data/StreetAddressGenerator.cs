namespace Cli.Data;

public class StreetAddressGenerator
{
    private static readonly string[] Prefixes =
    [
        "North", "South", "East", "West", "Old", "New", "Upper", "Lower", "Lake", "River",
        "Hill", "Valley", "Maple", "Oak", "Pine", "Cedar", "Elm", "Willow", "Cherry", "Birch",
        "Sunset", "Sunrise", "Forest", "Garden", "Park", "Spring", "Autumn", "Summer", "Winter", "Highland",
        "Meadow", "Grove", "Woodland", "Stone", "Brook", "Creek", "Bay", "Harbor", "Mountain", "Canyon"
    ];

    private static readonly string[] Nouns =
    [
        "Avenue", "Street", "Road", "Boulevard", "Lane", "Drive", "Court", "Circle", "Place", "Terrace",
        "Way", "Trail", "Parkway", "Commons", "Square", "Alley", "Crescent", "Row", "Path", "Plaza"
    ];

    private readonly Random _random = new(42);

    public string Next()
    {
        var prefix = Prefixes[_random.Next(Prefixes.Length)];
        var noun = Nouns[_random.Next(Nouns.Length)];
        var number = _random.Next(1, 200);
        return $"{prefix} {noun} {number}";
    }

    public IEnumerable<string> Addresses()
    {
        while (true) yield return Next();
    }
}
