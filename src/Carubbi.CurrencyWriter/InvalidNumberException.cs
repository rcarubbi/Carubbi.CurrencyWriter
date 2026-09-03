namespace Carubbi.CurrencyWriter;

public class InvalidNumberException : Exception
{
    public InvalidNumberException(string message)
        : base(message)
    {
    }
}
