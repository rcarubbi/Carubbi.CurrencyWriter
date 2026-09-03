using System.Globalization;

namespace Carubbi.CurrencyWriter;

public abstract class CurrencyWriterBase : ICurrencyWriter
{
    private const decimal MaxValue = 999999999999999.99M;

    protected CurrencyWriterBase(CultureInfo culture)
    {
        Culture = culture;
    }

    protected abstract string ApplyOrderIdentifiers(string valuePart, int order);

    protected abstract string[] UnionParts(string[] parts);

    protected abstract string ApplyCurrency(string[] parts, CurrencyType currencyType);

    protected abstract string WriteUnit(char digit);

    protected abstract string WriteDozen(char digit);

    protected abstract string WriteHundred(char digit);

    protected abstract string UnionDigits(string unit, string dozen, string hundred);

    protected virtual int[] SplitValue(decimal value)
    {
        int[] arrayReturn = new int[6];

        arrayReturn[0] = RetrieveCents(value);

        string strIntValue = Math.Floor(value).ToString(CultureInfo.InvariantCulture);

        int i = 1;

        while (strIntValue.Length >= 3)
        {
            arrayReturn[i++] = int.Parse(strIntValue.Substring(strIntValue.Length - 3, 3), CultureInfo.InvariantCulture);
            strIntValue = strIntValue.Substring(0, strIntValue.Length - 3);
        }

        if (strIntValue.Length > 0)
            arrayReturn[i] = int.Parse(strIntValue.Substring(0, strIntValue.Length), CultureInfo.InvariantCulture);

        return arrayReturn;
    }

    protected virtual int RetrieveCents(decimal value)
    {
        string separator = Culture.NumberFormat.NumberDecimalSeparator;
        string[] strCents = (value % 1).ToString(Culture).Split(separator);

        if (strCents.Length <= 1)
            return 0;

        string centsPart = strCents[1].PadRight(2, '0');
        return int.TryParse(centsPart, NumberStyles.None, CultureInfo.InvariantCulture, out int result)
            ? result
            : 0;
    }

    protected virtual string WritePart(int part)
    {
        char[] digits = part.ToString(CultureInfo.InvariantCulture).ToCharArray();

        string strUnit = WriteUnit(digits[digits.Length - 1]);
        string strDozen = digits.Length >= 2 ? WriteDozen(digits[digits.Length - 2]) : string.Empty;
        string strHundred = digits.Length == 3 ? WriteHundred(digits[digits.Length - 3]) : string.Empty;

        return UnionDigits(strUnit, strDozen, strHundred);
    }

    public string Write(decimal value, CurrencyType currencyType)
    {
        if (value <= 0)
            return string.Empty;

        Validate(value);

        int[] valueParts = SplitValue(value);
        string[] strValueParts = new string[6];

        for (int i = 0; i < valueParts.Length; i++)
        {
            string part = WritePart(valueParts[i]);
            strValueParts[i] = ApplyOrderIdentifiers(part, i);
        }

        string[] numberParts = UnionParts(strValueParts);
        string result = ApplyCurrency(numberParts, currencyType);

        return result[..1].ToUpper() + result[1..];
    }

    private static void Validate(decimal value)
    {
        if (value > MaxValue)
            throw new InvalidNumberException("Value exceeds the allowed limit.");
    }

    public CultureInfo Culture { get; set; }
}
