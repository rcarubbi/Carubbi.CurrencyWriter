using System.Globalization;

namespace Carubbi.CurrencyWriter;

public class CurrencyWriterPtBr : CurrencyWriterBase
{
    public CurrencyWriterPtBr(CultureInfo culture)
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
                    strOrder = valuePart == "um" ? "milhão" : "milhões";
                    break;
                case 4:
                    strOrder = valuePart == "um" ? "bilhão" : "bilhões";
                    break;
                case 5:
                    strOrder = valuePart == "um" ? "trilhão" : "trilhões";
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

            if (parts[i].Trim().Contains("cento e") ||
                parts[i].Trim().Contains("duzentos e") ||
                parts[i].Trim().Contains("trezentos e") ||
                parts[i].Trim().Contains("quatrocentos e") ||
                parts[i].Trim().Contains("quinhentos e") ||
                parts[i].Trim().Contains("seiscentos e") ||
                parts[i].Trim().Contains("setecentos e") ||
                parts[i].Trim().Contains("oitocentos e") ||
                parts[i].Trim().Contains("novecentos e"))
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
                ? $"e {parts[i]} "
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
            centName = parts[0].Trim() == "um" ? "centavo" : "centavos";

        if (!string.IsNullOrEmpty(parts[1]))
        {
            if (parts[1].Trim() != "um")
            {
                if (parts[1].Trim().EndsWith("lhões") || parts[1].Trim().EndsWith("lhão"))
                    currencyName = "de ";

                switch (currencyType)
                {
                    case CurrencyType.Real:
                        currencyName += "reais";
                        break;
                    case CurrencyType.Dollar:
                        currencyName += "dólares";
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
                        currencyName = "dólar";
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
                result += " e ";

            result += parts[0];
        }

        return result;
    }

    protected override string WriteUnit(char digit)
    {
        return int.Parse(digit.ToString()) switch
        {
            1 => "um",
            2 => "dois",
            3 => "três",
            4 => "quatro",
            5 => "cinco",
            6 => "seis",
            7 => "sete",
            8 => "oito",
            9 => "nove",
            _ => string.Empty
        };
    }

    protected override string WriteDozen(char digit)
    {
        return int.Parse(digit.ToString()) switch
        {
            1 => "dez",
            2 => "vinte",
            3 => "trinta",
            4 => "quarenta",
            5 => "cinquenta",
            6 => "sessenta",
            7 => "setenta",
            8 => "oitenta",
            9 => "noventa",
            _ => string.Empty
        };
    }

    protected override string WriteHundred(char digit)
    {
        return int.Parse(digit.ToString()) switch
        {
            1 => "cem",
            2 => "duzentos",
            3 => "trezentos",
            4 => "quatrocentos",
            5 => "quinhentos",
            6 => "seiscentos",
            7 => "setecentos",
            8 => "oitocentos",
            9 => "novecentos",
            _ => string.Empty
        };
    }

    protected override string UnionDigits(string unit, string dozen, string hundred)
    {
        string part1;
        string part2;

        if (dozen == "dez")
        {
            part1 = unit switch
            {
                "um" => "onze",
                "dois" => "doze",
                "três" => "treze",
                "quatro" => "quatorze",
                "cinco" => "quinze",
                "seis" => "dezesseis",
                "sete" => "dezessete",
                "oito" => "dezoito",
                "nove" => "dezenove",
                _ => string.Empty
            };

            part2 = string.IsNullOrEmpty(part1) ? dozen : string.Empty;
        }
        else
        {
            part1 = unit;
            part2 = dozen;
        }

        string part3 = hundred == "cem" && (!string.IsNullOrEmpty(part1) || !string.IsNullOrEmpty(part2))
            ? "cento"
            : hundred;

        if (!string.IsNullOrEmpty(part3) && (!string.IsNullOrEmpty(part2) || !string.IsNullOrEmpty(part1)))
            part3 += " e ";

        if (!string.IsNullOrEmpty(part2) && !string.IsNullOrEmpty(part1))
            part2 += " e ";

        return $"{part3}{part2}{part1}";
    }
}
