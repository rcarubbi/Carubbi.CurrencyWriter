using System.Globalization;
using Carubbi.CurrencyWriter;

namespace Carubbi.CurrencyWriter.Tests;

public class CurrencyWriterEsEsTests
{
    private CurrencyWriterEsES _sut = null!;

    [Before(Test)]
    public void CreateSut() => _sut = new CurrencyWriterEsES(new CultureInfo("es-ES"));

    [Test]
    public async Task Write_When_OnePeso_Then_ReturnsSingularCurrency()
    {
        var result = _sut.Write(1.00m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Un peso");
    }

    [Test]
    public async Task Write_When_TwoPesos_Then_ReturnsPluralCurrency()
    {
        var result = _sut.Write(2.00m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Dos pesos");
    }

    [Test]
    public async Task Write_When_Seventeen_Then_ReturnsDiecisiete()
    {
        var result = _sut.Write(17m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Diecisiete pesos");
    }

    [Test]
    public async Task Write_When_OneHundredAndOne_Then_ReturnsCientoUn()
    {
        var result = _sut.Write(101m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Ciento un pesos");
    }

    [Test]
    public async Task Write_When_OneMillion_Then_ReturnsUnMillonDePesos()
    {
        var result = _sut.Write(1000000m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Un millón de pesos");
    }

    [Test]
    public async Task Write_When_ComposedNumber_Then_ReturnsFullForm()
    {
        var result = _sut.Write(1234567.89m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Un millón doscientos treinta y cuatro mil quinientos sesenta y siete pesos con ochenta y nueve centavos");
    }

    [Test]
    public async Task Write_When_OnePesoFiftyCents_Then_ReturnsConCincuentaCentavos()
    {
        var result = _sut.Write(1.50m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Un peso con cincuenta centavos");
    }

    [Test]
    public async Task Write_When_FiftyCents_Then_ReturnsOnlyCents()
    {
        var result = _sut.Write(0.50m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Cincuenta centavos");
    }

    [Test]
    public async Task Write_When_OneCent_Then_ReturnsSingularCent()
    {
        var result = _sut.Write(1.01m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Un peso con un centavo");
    }

    [Test]
    public async Task Write_When_TwentyOne_Then_UsesYConjunction()
    {
        var result = _sut.Write(21m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Veinte y un pesos");
    }

    [Test]
    public async Task Write_When_ComposedCents_Then_ReturnsConAndCents()
    {
        var result = _sut.Write(33.33m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo("Treinta y tres pesos con treinta y tres centavos");
    }

    [Test]
    public async Task Write_When_Zero_Then_ReturnsEmptyString()
    {
        var result = _sut.Write(0m, CurrencyType.Peso);

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Write_When_AboveLimit_Then_ThrowsInvalidNumberException()
    {
        await Assert.That(() => _sut.Write(1000000000000000m, CurrencyType.Peso)).Throws<InvalidNumberException>();
    }
}
