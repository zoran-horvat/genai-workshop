using System;
using System.Collections.Generic;
using Web.Data.Abstractions;
using Web.Models;

namespace Cli.Data;

public class AddressGenerator(StreetAddressGenerator streetNameGenerator)
{
    private static readonly (string town, string country)[] Towns =
    [
        ("Hallstatt", "Austria"), ("Giethoorn", "Netherlands"), ("Gruyères", "Switzerland"), ("Colmar", "France"),
        ("Ronda", "Spain"), ("Bled", "Slovenia"), ("Reine", "Norway"), ("Eguisheim", "France"), ("Bibury", "England"),
        ("Goreme", "Turkey"), ("Cochem", "Germany"), ("Kotor", "Montenegro"), ("Soglio", "Switzerland"),
        ("Rothenburg ob der Tauber", "Germany"), ("Manarola", "Italy"), ("Oia", "Greece"), ("Český Krumlov", "Czechia"),
        ("Dinant", "Belgium"), ("Albarracín", "Spain"), ("Saint-Cirq-Lapopie", "France"), ("Portree", "Scotland"),
        ("Piran", "Slovenia"), ("Monschau", "Germany"), ("Szentendre", "Hungary"), ("Lavenham", "England"),
        ("Lofoten", "Norway"), ("Lauterbrunnen", "Switzerland"), ("Mittenwald", "Germany"), ("Civita di Bagnoregio", "Italy"),
        ("Vik", "Iceland"), ("Eze", "France"), ("Saint-Paul-de-Vence", "France"), ("Cassis", "France"), ("Polperro", "England"),
        ("Tenby", "Wales"), ("Dinan", "France"), ("Riquewihr", "France"), ("Positano", "Italy"), ("Sibiu", "Romania"),
        ("Kinsale", "Ireland"), ("Tihany", "Hungary"), ("Valldemossa", "Spain"), ("San Gimignano", "Italy"), ("Annecy", "France"),
        ("Potes", "Spain"), ("Tallinn", "Estonia"), ("Bruges", "Belgium"), ("Mdina", "Malta"), ("Füssen", "Germany"),
        ("Potes", "Spain"), ("Bamberg", "Germany"), ("Gjirokastër", "Albania"), ("České Budějovice", "Czechia"),
        ("Kotor", "Montenegro"), ("Szentendre", "Hungary"), ("Lindau", "Germany"), ("Bamberg", "Germany"), ("Bergen", "Norway"),
        ("Ceský Krumlov", "Czechia"), ("Sighisoara", "Romania"), ("Kotor", "Montenegro"), ("Banska Stiavnica", "Slovakia"),
        ("Banska Bystrica", "Slovakia"), ("Banska Štiavnica", "Slovakia"), ("Bled", "Slovenia"), ("Bosa", "Italy"),
        ("Bovec", "Slovenia"), ("Brixen", "Italy"), ("Bruges", "Belgium"), ("Cefalù", "Italy"), ("Chamonix", "France"),
        ("Chesky Krumlov", "Czechia"), ("Chur", "Switzerland"), ("Cochem", "Germany"), ("Colmar", "France"),
        ("Cortina d'Ampezzo", "Italy"), ("Cudillero", "Spain"), ("Dinan", "France")
    ];

    private readonly StreetAddressGenerator _streetAddressGenerator = streetNameGenerator;
    private readonly Random _random = new Random(42);

    public Address Next()
    {
        var postalCode = _random.Next(10000, 100000).ToString();
        var (town, country) = Towns[_random.Next(Towns.Length)];
        var addressKind = RandomAddressKind();
        return new Address(
            ExternalId<Address>.CreateNew(),
            _streetAddressGenerator.Next(),
            town,
            "-",
            postalCode,
            country,
            addressKind
        );
    }

    private AddressKind RandomAddressKind() =>
        Enum.GetValues<AddressKind>()
            .Except([ AddressKind.Default, AddressKind.Other ])
            .Aggregate((AddressKind)0, (current, kind) => current | (_random.NextDouble() < 0.5 ? kind : 0));

    public IEnumerable<Address> Addresses()
    {
        while (true) yield return Next();
    }
}
