# Carubbi.CurrencyWriter

[![NuGet](https://img.shields.io/nuget/v/Carubbi.CurrencyWriter)](https://www.nuget.org/packages/Carubbi.CurrencyWriter)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Carubbi.CurrencyWriter)](https://www.nuget.org/packages/Carubbi.CurrencyWriter)

A multi-language library to write a currency value in full form.

> This component writes a currency value in full form. It supports English, Spanish and Brazilian Portuguese and is able to write values from 0 up to 999 trillions. It has four currency types: Dollar, Real, Euro and Peso.

## Projects

| Project | Package |
|---------|---------|
| `Carubbi.CurrencyWriter` | `Carubbi.CurrencyWriter` |

Target framework: `net10.0`. Requires .NET 10 SDK.

## Usage

Create a writer through the `CurrencyWriterFactory` by passing the target culture, then call `Write` with the value and the `CurrencyType`:

```csharp
using System.Globalization;
using Carubbi.CurrencyWriter;

var writer = CurrencyWriterFactory.GetCurrencyWriter(new CultureInfo("pt-BR"));

string full = writer.Write(1234.56m, CurrencyType.Real);
// "Um mil duzentos e trinta e quatro reais e cinquenta e seis centavos"
```

### Brazilian Portuguese

```csharp
var ptWriter = CurrencyWriterFactory.GetCurrencyWriter(new CultureInfo("pt-BR"));
ptWriter.Write(1.99m, CurrencyType.Real);  // "Um real e noventa e nove centavos"
ptWriter.Write(1000m, CurrencyType.Real);  // "Um mil reais"
```

### English (US)

```csharp
var enWriter = CurrencyWriterFactory.GetCurrencyWriter(new CultureInfo("en-US"));
enWriter.Write(17.50m, CurrencyType.Dollar);  // "Seventeen dollars and a half"
enWriter.Write(21.25m, CurrencyType.Dollar);  // "Twenty-one dollars and a quarter"
```

### Spanish (ES)

```csharp
var esWriter = CurrencyWriterFactory.GetCurrencyWriter(new CultureInfo("es-ES"));
esWriter.Write(33.33m, CurrencyType.Peso);  // "Treinta y tres pesos con treinta y tres centavos"
```

### Factory API

- `CurrencyWriterFactory.Instance` — shared singleton instance.
- `GetCurrencyWriter(CultureInfo)` — returns the writer for `pt`/`pt-BR`, `en`/`en-US` and `es`/`es-ES`/`es-CL`; throws `NotImplementedException` otherwise.
- `ListCultures()` — all supported cultures.
- `ListCurrencies()` — all supported `CurrencyType` values.

Values above `999,999,999,999,999.99` throw `InvalidNumberException`.

## Building and testing locally

Prerequisites: .NET SDK 10.

```shell
dotnet build Carubbi.CurrencyWriter.slnx -c Release
dotnet run --project tests/Carubbi.CurrencyWriter.Tests/Carubbi.CurrencyWriter.Tests.csproj -c Release
```

## Releasing

Pushing a tag `v*` (for example `v2.0.0`) triggers the `publish` workflow, which builds in Release mode, packs the library and publishes it to nuget.org using GitHub Actions trusted publishing.
