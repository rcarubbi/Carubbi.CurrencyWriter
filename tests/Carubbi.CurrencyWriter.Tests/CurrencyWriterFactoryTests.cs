using System.Globalization;
using Carubbi.CurrencyWriter;

namespace Carubbi.CurrencyWriter.Tests;

public class CurrencyWriterFactoryTests
{
    [Test]
    public async Task GetInstance_When_Called_Then_ReturnsSameInstance()
    {
        var first = CurrencyWriterFactory.Instance;
        var second = CurrencyWriterFactory.Instance;

        await Assert.That(ReferenceEquals(first, second)).IsTrue();
    }

    [Test]
    public async Task GetCurrencyWriter_When_PtBrCulture_Then_ReturnsPtBrWriter()
    {
        var writer = CurrencyWriterFactory.GetCurrencyWriter(new CultureInfo("pt-BR"));

        await Assert.That(writer).IsTypeOf<CurrencyWriterPtBr>();
    }

    [Test]
    public async Task GetCurrencyWriter_When_EnUsCulture_Then_ReturnsEnUsWriter()
    {
        var writer = CurrencyWriterFactory.GetCurrencyWriter(new CultureInfo("en-US"));

        await Assert.That(writer).IsTypeOf<CurrencyWriterEnUS>();
    }

    [Test]
    public async Task GetCurrencyWriter_When_EsEsCulture_Then_ReturnsEsEsWriter()
    {
        var writer = CurrencyWriterFactory.GetCurrencyWriter(new CultureInfo("es-ES"));

        await Assert.That(writer).IsTypeOf<CurrencyWriterEsES>();
    }

    [Test]
    public async Task GetCurrencyWriter_When_EsClCulture_Then_ReturnsEsEsWriter()
    {
        var writer = CurrencyWriterFactory.GetCurrencyWriter(new CultureInfo("es-CL"));

        await Assert.That(writer).IsTypeOf<CurrencyWriterEsES>();
    }

    [Test]
    public async Task GetCurrencyWriter_When_UnsupportedCulture_Then_ThrowsNotImplementedException()
    {
        await Assert.That(() => CurrencyWriterFactory.GetCurrencyWriter(new CultureInfo("fr-FR"))).Throws<NotImplementedException>();
    }

    [Test]
    public async Task ListCultures_When_Called_Then_ReturnsAllSupportedCultures()
    {
        var cultures = CurrencyWriterFactory.ListCultures();

        await Assert.That(cultures.Select(c => c.Name)).IsEquivalentTo(new[] { "pt", "pt-BR", "en", "en-US", "es", "es-ES", "es-CL" });
    }

    [Test]
    public async Task ListCurrencies_When_Called_Then_ReturnsAllCurrencies()
    {
        var currencies = CurrencyWriterFactory.ListCurrencies();

        await Assert.That(currencies).IsEquivalentTo(new[] { CurrencyType.Real, CurrencyType.Dollar, CurrencyType.Euro, CurrencyType.Peso });
    }
}
