using System.Globalization;

namespace Carubbi.CurrencyWriter;

public interface ICurrencyWriter
{
    string Write(decimal value, CurrencyType currencyType);

    CultureInfo Culture { get; set; }
}
