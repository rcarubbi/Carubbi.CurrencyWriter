using System.Globalization;
using Carubbi.CurrencyWriter;

namespace Carubbi.CurrencyWriter.Tests;

public class CurrencyWriterEnUsTests
{
    private CurrencyWriterEnUS _sut = null!;

    [Before(Test)]
    public void CreateSut() => _sut = new CurrencyWriterEnUS(new CultureInfo("en-US"));

    [Test]
    public async Task Write_When_OneDollar_Then_ReturnsSingularCurrency()
    {
        var result = _sut.Write(1.00m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("One dollar");
    }

    [Test]
    public async Task Write_When_TwoDollars_Then_ReturnsPluralCurrency()
    {
        var result = _sut.Write(2.00m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("Two dollars");
    }

    [Test]
    public async Task Write_When_SeventeenDollars_Then_ReturnsSeventeen()
    {
        var result = _sut.Write(17m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("Seventeen dollars");
    }

    [Test]
    public async Task Write_When_TwentyOneDollars_Then_DashesTensAndUnits()
    {
        var result = _sut.Write(21m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("Twenty-one dollars");
    }

    [Test]
    public async Task Write_When_OneHundredAndOne_Then_UsesAndConjunction()
    {
        var result = _sut.Write(101m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("One hundred and one dollars");
    }

    [Test]
    public async Task Write_When_OneMillion_Then_ReturnsOneMillionDollars()
    {
        var result = _sut.Write(1000000m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("One million dollars");
    }

    [Test]
    public async Task Write_When_ComposedNumber_Then_ReturnsFullForm()
    {
        var result = _sut.Write(1234567.89m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("One million two hundred and thirty-four thousand five hundred and sixty-seven dollars and eighty-nine cents");
    }

    [Test]
    public async Task Write_When_OneDollarFiftyCents_Then_ReturnsAHalf()
    {
        var result = _sut.Write(1.50m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("One dollar and a half");
    }

    [Test]
    public async Task Write_When_OneCent_Then_ReturnsAPenny()
    {
        var result = _sut.Write(1.01m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("One dollar and a penny");
    }

    [Test]
    public async Task Write_When_TwentyFiveCents_Then_ReturnsAQuarter()
    {
        var result = _sut.Write(0.25m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("A quarter");
    }

    [Test]
    public async Task Write_When_FiftyCents_Then_ReturnsAHalf()
    {
        var result = _sut.Write(0.50m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("A half");
    }

    [Test]
    public async Task Write_When_TenCents_Then_ReturnsADime()
    {
        var result = _sut.Write(0.10m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("A dime");
    }

    [Test]
    public async Task Write_When_ComposedCents_Then_ReturnsCents()
    {
        var result = _sut.Write(33.33m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo("Thirty-three dollars and thirty-three cents");
    }

    [Test]
    public async Task Write_When_Zero_Then_ReturnsEmptyString()
    {
        var result = _sut.Write(0m, CurrencyType.Dollar);

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Write_When_AboveLimit_Then_ThrowsInvalidNumberException()
    {
        await Assert.That(() => _sut.Write(1000000000000000m, CurrencyType.Dollar)).Throws<InvalidNumberException>();
    }
}
