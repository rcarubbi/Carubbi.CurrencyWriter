using System.Globalization;

namespace Carubbi.CurrencyWriter;

public class CurrencyWriterEnUS : CurrencyWriterBase
{
    public CurrencyWriterEnUS(CultureInfo culture)
        : base(culture)
    {
    }

    protected override string ApplyOrderIdentifiers(string valuePart, int order)
    {
        string strOrder = string.Empty;
        if (!string.IsNullOrEmpty(valuePart))
        {
            switch (order)
            {
                case 2:
                    strOrder = "thousand";
                    break;
                case 3:
                    strOrder = valuePart == "one" ? "million" : "millions";
                    break;
                case 4:
                    strOrder = valuePart == "one" ? "billion" : "billions";
                    break;
                case 5:
                    strOrder = valuePart == "one" ? "trillion" : "trillions";
                    break;
            }
        }

        if (!string.IsNullOrEmpty(strOrder))
            valuePart += $" {strOrder}";

        return valuePart;
    }

    protected override string[] UnionParts(string[] parts)
    {
        string[] result = new string[2];

        result[0] = parts[0] + " ";

        int indiceConjuncao = -1;

        for (int i = 1; i < parts.Length; i++)
        {
            if (string.IsNullOrEmpty(parts[i]))
                continue;

            bool hasMoreParts = false;
            for (int j = i + 1; j < parts.Length; j++)
            {
                if (string.IsNullOrEmpty(parts[j]))
                    continue;

                hasMoreParts = true;
                break;
            }

            if (hasMoreParts && string.IsNullOrEmpty(parts[0]))
            {
                indiceConjuncao = i;
                break;
            }
        }

        for (int i = parts.Length - 1; i > 0; i--)
        {
            if (string.IsNullOrEmpty(parts[i].Trim()))
                continue;

            result[1] += i == indiceConjuncao
                ? $"and {parts[i]} "
                : parts[i] + " ";
        }

        return result;
    }

    protected override string ApplyCurrency(string[] parts, CurrencyType currencyType)
    {
        string centName = string.Empty;
        string currencyName = string.Empty;
        string result = string.Empty;

        if (!string.IsNullOrEmpty(parts[0].Trim()))
        {
            if (currencyType == CurrencyType.Dollar)
            {
                switch (parts[0].Trim())
                {
                    case "one":
                        parts[0] = "";
                        centName = "a penny";
                        break;
                    case "ten":
                        parts[0] = "";
                        centName = "a dime";
                        break;
                    case "fifty":
                        parts[0] = "";
                        centName = "a half";
                        break;
                    case "twenty-five":
                        parts[0] = "";
                        centName = "a quarter";
                        break;
                    default:
                        centName = "cents";
                        break;
                }
            }
            else if (currencyType == CurrencyType.Real)
            {
                centName = parts[0].Trim() == "one" ? "cent" : "cents";
            }
        }

        if (!string.IsNullOrEmpty(parts[1]))
        {
            switch (currencyType)
            {
                case CurrencyType.Dollar:
                    currencyName = parts[1].Trim() != "one" ? "dollars" : "dollar";
                    break;
                case CurrencyType.Real:
                    currencyName = parts[1].Trim() != "one" ? "reals" : "real";
                    break;
                case CurrencyType.Peso:
                    currencyName = parts[1].Trim() != "one" ? "pesos" : "peso";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        parts[0] += centName;
        parts[1] += currencyName;

        if (!string.IsNullOrEmpty(parts[1].Trim()))
            result = parts[1];

        if (!string.IsNullOrEmpty(parts[0].Trim()))
        {
            if (!string.IsNullOrEmpty(result))
                result += " and ";

            result += parts[0];
        }

        return result;
    }

    protected override string WriteUnit(char digit)
    {
        return int.Parse(digit.ToString()) switch
        {
            1 => "one",
            2 => "two",
            3 => "three",
            4 => "four",
            5 => "five",
            6 => "six",
            7 => "seven",
            8 => "eight",
            9 => "nine",
            _ => string.Empty
        };
    }

    protected override string WriteDozen(char digit)
    {
        return int.Parse(digit.ToString()) switch
        {
            1 => "ten",
            2 => "twenty",
            3 => "thirty",
            4 => "forty",
            5 => "fifty",
            6 => "sixty",
            7 => "seventy",
            8 => "eighty",
            9 => "ninety",
            _ => string.Empty
        };
    }

    protected override string WriteHundred(char digit) => $"{WriteUnit(digit)} hundred";

    protected override string UnionDigits(string unit, string dozen, string hundred)
    {
        string part1;
        string part2;

        if (dozen == "ten")
        {
            part1 = unit switch
            {
                "one" => "eleven",
                "two" => "twelve",
                "three" => "thirteen",
                "four" => "fourteen",
                "five" => "fifteen",
                "six" => "sixteen",
                "seven" => "seventeen",
                "eight" => "eighteen",
                "nine" => "nineteen",
                _ => string.Empty
            };

            part2 = string.IsNullOrEmpty(part1) ? dozen : string.Empty;
        }
        else
        {
            part1 = unit;
            part2 = dozen;
        }

        string part3 = hundred;

        if (!string.IsNullOrEmpty(part3) && (!string.IsNullOrEmpty(part2) || !string.IsNullOrEmpty(part1)))
            part3 += " and ";

        if (!string.IsNullOrEmpty(part2) && !string.IsNullOrEmpty(part1))
            part2 += "-";

        return $"{part3}{part2}{part1}";
    }
}
