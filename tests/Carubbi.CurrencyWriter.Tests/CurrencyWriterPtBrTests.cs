using System.Globalization;
using Carubbi.CurrencyWriter;

namespace Carubbi.CurrencyWriter.Tests;

public class CurrencyWriterPtBrTests
{
    private CurrencyWriterPtBr _sut = null!;

    [Before(Test)]
    public void CreateSut() => _sut = new CurrencyWriterPtBr(new CultureInfo("pt-BR"));

    [Test]
    public async Task Write_When_OneReal_Then_ReturnsSingularCurrency()
    {
        var result = _sut.Write(1.00m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Um real");
    }

    [Test]
    public async Task Write_When_TwoReals_Then_ReturnsPluralCurrency()
    {
        var result = _sut.Write(2.00m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Dois reais");
    }

    [Test]
    public async Task Write_When_TenReals_Then_ReturnsTen()
    {
        var result = _sut.Write(10m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Dez reais");
    }

    [Test]
    public async Task Write_When_SeventeenReals_Then_ReturnsDezessete()
    {
        var result = _sut.Write(17m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Dezessete reais");
    }

    [Test]
    public async Task Write_When_LargeHundreds_Then_ReturnsCentoE()
    {
        var result = _sut.Write(101m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Cento e um reais");
    }

    [Test]
    public async Task Write_When_OneThousand_Then_ReturnsUmMil()
    {
        var result = _sut.Write(1000m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Um mil reais");
    }

    [Test]
    public async Task Write_When_OneMillion_Then_ReturnsUmMilhaoDeReais()
    {
        var result = _sut.Write(1000000m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Um milhão de reais");
    }

    [Test]
    public async Task Write_When_ComposedNumber_Then_ReturnsFullForm()
    {
        var result = _sut.Write(1234567.89m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Um milhão duzentos e trinta e quatro mil quinhentos e sessenta e sete reais e oitenta e nove centavos");
    }

    [Test]
    public async Task Write_When_OneRealFiftyCents_Then_ReturnsCurrencyAndCents()
    {
        var result = _sut.Write(1.50m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Um real e cinquenta centavos");
    }

    [Test]
    public async Task Write_When_FiftyCents_Then_ReturnsOnlyCents()
    {
        var result = _sut.Write(0.50m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Cinquenta centavos");
    }

    [Test]
    public async Task Write_When_OneCent_Then_ReturnsSingularCent()
    {
        var result = _sut.Write(1.01m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Um real e um centavo");
    }

    [Test]
    public async Task Write_When_TwentyOneReals_Then_UsesEConjunction()
    {
        var result = _sut.Write(21m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo("Vinte e um reais");
    }

    [Test]
    public async Task Write_When_Zero_Then_ReturnsEmptyString()
    {
        var result = _sut.Write(0m, CurrencyType.Real);

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Write_When_AboveLimit_Then_ThrowsInvalidNumberException()
    {
        await Assert.That(() => _sut.Write(1000000000000000m, CurrencyType.Real)).Throws<InvalidNumberException>();
    }

    [Test]
    public async Task Culture_When_GivenEnUsCulture_Then_StoresCulture()
    {
        _sut.Culture = new CultureInfo("en-US");

        await Assert.That(_sut.Culture.Name).IsEqualTo("en-US");
    }
}
