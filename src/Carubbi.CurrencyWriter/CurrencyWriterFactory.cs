using System.Globalization;

namespace Carubbi.CurrencyWriter;

public class CurrencyWriterFactory
{
    private CurrencyWriterFactory()
    {
    }

    public static CurrencyWriterFactory Instance { get; } = new();

    public static IReadOnlyList<CultureInfo> ListCultures() =>
    [
        new("pt"),
        new("pt-BR"),
        new("en"),
        new("en-US"),
        new("es"),
        new("es-ES"),
        new("es-CL")
    ];

    public static IReadOnlyList<CurrencyType> ListCurrencies() =>
    [
        CurrencyType.Real,
        CurrencyType.Dollar,
        CurrencyType.Euro,
        CurrencyType.Peso
    ];

    public static ICurrencyWriter GetCurrencyWriter(CultureInfo culture)
    {
        return culture.Name switch
        {
            "pt" or "pt-BR" => new CurrencyWriterPtBr(culture),
            "en" or "en-US" => new CurrencyWriterEnUS(culture),
            "es" or "es-ES" or "es-CL" => new CurrencyWriterEsES(culture),
            _ => throw new NotImplementedException($"No writer registered for culture '{culture.Name}'.")
        };
    }
}
