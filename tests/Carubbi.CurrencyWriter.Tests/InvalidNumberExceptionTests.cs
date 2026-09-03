using Carubbi.CurrencyWriter;

namespace Carubbi.CurrencyWriter.Tests;

public class InvalidNumberExceptionTests
{
    [Test]
    public async Task Constructor_When_GivenMessage_Then_StoresMessage()
    {
        var exception = new InvalidNumberException("Value exceeds the allowed limit.");

        await Assert.That(exception.Message).IsEqualTo("Value exceeds the allowed limit.");
    }

    [Test]
    public async Task Constructor_When_Created_Then_IsException()
    {
        var exception = new InvalidNumberException("Message");

        await Assert.That(exception).IsTypeOf<InvalidNumberException>();
        await Assert.That(exception).IsAssignableTo<Exception>();
    }
}
