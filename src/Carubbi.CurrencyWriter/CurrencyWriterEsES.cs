using System.Globalization;

namespace Carubbi.CurrencyWriter;

public class CurrencyWriterEsES : CurrencyWriterBase
{
    public CurrencyWriterEsES(CultureInfo culture)
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
                    strOrder = "mil";
                    break;
                case 3:
                    strOrder = valuePart == "un" ? "millón" : "millones";
                    break;
                case 4:
                    strOrder = valuePart == "un" ? "billón" : "billones";
                    break;
                case 5:
                    strOrder = valuePart == "un" ? "trillón" : "trillones";
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

            if (parts[i].Trim().Contains("ciento ") ||
                parts[i].Trim().Contains("doscientos ") ||
                parts[i].Trim().Contains("trescientos ") ||
                parts[i].Trim().Contains("cuatrocientos ") ||
                parts[i].Trim().Contains("quinientos ") ||
                parts[i].Trim().Contains("seiscientos ") ||
                parts[i].Trim().Contains("setecientos ") ||
                parts[i].Trim().Contains("ochocientos ") ||
                parts[i].Trim().Contains("nuevecientos "))
                break;

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
                ? $" {parts[i]} "
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
            centName = parts[0].Trim() != "un" ? "centavos" : "centavo";

        if (!string.IsNullOrEmpty(parts[1]))
        {
            if (parts[1].Trim() != "un")
            {
                if (parts[1].Trim().EndsWith("llones") || parts[1].Trim().EndsWith("llón"))
                    currencyName = "de ";

                switch (currencyType)
                {
                    case CurrencyType.Real:
                        currencyName += "reales";
                        break;
                    case CurrencyType.Dollar:
                        currencyName += "dolares";
                        break;
                    case CurrencyType.Peso:
                        currencyName += "pesos";
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                switch (currencyType)
                {
                    case CurrencyType.Real:
                        currencyName = "real";
                        break;
                    case CurrencyType.Dollar:
                        currencyName = "dolar";
                        break;
                    case CurrencyType.Peso:
                        currencyName = "peso";
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        parts[0] += centName;
        parts[1] += currencyName;

        if (!string.IsNullOrEmpty(parts[1].Trim()))
            result = parts[1];

        if (!string.IsNullOrEmpty(parts[0].Trim()))
        {
            if (!string.IsNullOrEmpty(result))
                result += " con ";

            result += parts[0];
        }

        return result;
    }

    protected override string WriteUnit(char digit)
    {
        return int.Parse(digit.ToString()) switch
        {
            1 => "un",
            2 => "dos",
            3 => "tres",
            4 => "cuatro",
            5 => "cinco",
            6 => "seis",
            7 => "siete",
            8 => "ocho",
            9 => "nueve",
            _ => string.Empty
        };
    }

    protected override string WriteDozen(char digit)
    {
        return int.Parse(digit.ToString()) switch
        {
            1 => "diez",
            2 => "veinte",
            3 => "treinta",
            4 => "cuarenta",
            5 => "cincuenta",
            6 => "sesenta",
            7 => "setenta",
            8 => "ochenta",
            9 => "noventa",
            _ => string.Empty
        };
    }

    protected override string WriteHundred(char digit)
    {
        return int.Parse(digit.ToString()) switch
        {
            1 => "cien",
            2 or 3 or 4 or 6 or 8 or 9 => WriteUnit(digit) + "cientos",
            5 => "quinientos",
            7 => "setecientos",
            _ => string.Empty
        };
    }

    protected override string UnionDigits(string unit, string dozen, string hundred)
    {
        string part1;
        string part2;

        if (dozen == "diez")
        {
            part1 = unit switch
            {
                "un" => "once",
                "dos" => "doce",
                "tres" => "trece",
                "cuatro" => "catorce",
                "cinco" => "quince",
                "seis" => "dieciséis",
                "siete" => "diecisiete",
                "ocho" => "dieciocho",
                "nueve" => "diecinueve",
                _ => string.Empty
            };

            part2 = string.IsNullOrEmpty(part1) ? dozen : string.Empty;
        }
        else
        {
            part1 = unit;
            part2 = dozen;
        }

        string part3 = hundred == "cien" && (!string.IsNullOrEmpty(part1) || !string.IsNullOrEmpty(part2))
            ? "ciento"
            : hundred;

        if (!string.IsNullOrEmpty(part3) && (!string.IsNullOrEmpty(part2) || !string.IsNullOrEmpty(part1)))
            part3 += " ";

        if (!string.IsNullOrEmpty(part2) && !string.IsNullOrEmpty(part1))
            part2 += " y ";

        return $"{part3}{part2}{part1}";
    }
}
