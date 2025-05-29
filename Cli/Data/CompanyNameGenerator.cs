namespace Cli.Data;

public class CompanyNameGenerator
{
    private static readonly string[] Adjectives =
    [
        "Global", "Dynamic", "Innovative", "Prime", "Elite", "NextGen", "Pioneer", "Trusted", "Agile", "Bright",
        "Creative", "Modern", "Smart", "Rapid", "Solid", "United", "Visionary", "Bold", "Noble", "Peak",
        "Vivid", "Epic", "True", "Blue", "Green", "Red", "Golden", "Silver", "Quantum", "Alpha",
        "Omega", "Apex", "Summit", "Urban", "Rural", "Metro", "Digital", "Analog", "Classic", "Future",
        "Heritage", "Legacy", "Core", "Infinite", "Superior", "Majestic", "Grand", "Supreme", "Royal", "Crystal",
        "Harmony", "Magnetic", "Magna", "Stellar", "Nova", "Radiant", "Luminous", "Brilliant", "Clever", "Swift",
        "Nimble", "Robust", "Steadfast", "Mighty", "Valiant", "Courageous", "Daring", "Resilient", "Enduring", "Steady",
        "Serene", "Tranquil", "Peaceful", "Sunny", "Lively", "Cheerful", "Lucky", "Fortune", "Prosperous", "Opulent",
        "Resourceful", "Inventive", "Versatile", "Flexible", "Balanced", "Efficient", "Energetic", "Ambitious", "Driven", "Passionate",
        "Zealous", "Enthusiastic", "Diligent", "Meticulous", "Precise", "Thorough", "Attentive", "Observant", "Astute", "Sharp"
    ];

    private static readonly string[] Nouns =
    [
        "Solutions", "Systems", "Technologies", "Partners", "Ventures", "Enterprises", "Group", "Holdings", "Consulting", "Logistics",
        "Industries", "Services", "Networks", "Resources", "Dynamics", "Concepts", "Associates", "Works", "Labs", "Media",
        "Designs", "Projects", "Products", "Markets", "Foods", "Energy", "Finance", "Capital", "Realty", "Properties",
        "Health", "Care", "Retail", "Supply", "Distribution", "Development", "Innovation", "Analytics", "Security", "Data",
        "Cloud", "Edge", "Mobility", "Transport", "Trade", "Manufacturing", "Consultants", "Advisors", "Builders", "Solutions",
        "Studios", "Creations", "Events", "Brands", "Motors", "Auto", "Fleet", "Warehousing", "Textiles", "Fashion",
        "Digital", "Print", "Publishing", "Education", "Learning", "Academy", "School", "Clinic", "Pharma", "Biotech",
        "Mining", "Metals", "Chemicals", "Plastics", "Packaging", "Furniture", "Home", "Garden", "Travel", "Tourism",
        "Hospitality", "Resorts", "Leisure", "Sports", "Fitness", "Wellness", "Beauty", "Art", "Crafts", "Music"
    ];

    private static readonly string[] EntityTypes =
    [
        "LLC", "Inc", "Ltd", "Corp", "GmbH", "S.A.", "PLC", "Pty Ltd", "Sarl", "BV", "LLP", "LP", "SNC", "KG", "K.K.", "Oy", "AB", "AS", "SpA", "S.p.r.l.",
        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
    ];

    private readonly Random _random = new(42);

    public string Next()
    {
        var adjective = Adjectives[_random.Next(Adjectives.Length)];
        var noun = Nouns[_random.Next(Nouns.Length)];
        var entityType = EntityTypes[_random.Next(EntityTypes.Length)];
        return string.Join(" ", new[] { adjective, noun, entityType }.Where(part => !string.IsNullOrWhiteSpace(part)));
    }

    public IEnumerable<string> Names()
    {
        while (true) yield return Next();
    }
}
