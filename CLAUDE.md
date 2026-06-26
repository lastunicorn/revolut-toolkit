# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

A .NET 8.0 library (`DustInTheWind.Revolut.Toolkit`) published as a NuGet package for parsing CSV statement exports from Revolut bank. The CSV format has the header: `Type,Product,Started Date,Completed Date,Description,Amount,Fee,Currency,State,Balance`.

## Commands

```bash
# Build
dotnet build ./Revolut.Toolkit.slnx -c Release

# Run all tests
dotnet test ./Revolut.Toolkit.slnx

# Run a single test project
dotnet test sources/Revolut.Toolkit.Tests/Revolut.Toolkit.Tests.csproj

# Run the demo
dotnet run --project sources/Revolut.Toolkit.Demo
```

## Project Structure

- `sources/Revolut.Toolkit/` — main library; everything here is NuGet-packaged public API
- `sources/Revolut.Toolkit.Tests/` — xUnit test project (not packaged)
- `sources/Revolut.Toolkit.Demo/` — CLI demo reading `statement.csv`

Assembly naming convention: `DustInTheWind.$(MSBuildProjectName)` (defined in each `.csproj`).

## Architecture

**Public API surface:**
- `StatementDocument` — extends `Collection<BankTransaction>`; entry point via static async `LoadFromFileAsync`, `LoadAsync(string)`, `LoadAsync(Stream)`, `LoadAsync(FileInfo)`, `LoadAsync(StreamReader)`, `LoadAsync(TextReader)`
- `BankTransaction` — `record class` mapping each CSV row
- `TransactionType`, `TransactionProduct`, `TransactionCurrency`, `TransactionState` — sealed record value objects with static `KnownValues` collection and implicit `string` conversions

**Internal CSV layer** (`Revolut.Toolkit/Csv/`):
- `CsvStatementDocument` — wraps CsvHelper, manages read state (`HeaderRow` → `DataRow` → `Ended`), yields `BankTransaction` via `IAsyncEnumerable`
- `BankTransactionMap` — CsvHelper `ClassMap` wiring column names to properties; dates parsed as `"yyyy-MM-dd HH:mm:ss"`
- Per-type converters: `TransactionTypeConverter`, `TransactionProductConverter`, `TransactionCurrencyConverter`, `TransactionStateConverter`

**Exception hierarchy:**
- `DocumentLoadException` (base) → `HeaderLoadException`, `DataLoadException`
- `StatementDocument` re-wraps all non-`DocumentLoadException` exceptions before propagating

## Code Conventions

- Never use `var`; always use the explicit type.
- LINQ lambda parameter: `x`.
- Object instantiation: prefer `new()` (target-typed).
- No curly braces for single-line `if`, `for`, `using` bodies.
- No underscore prefix on fields.
- XML documentation only for public types exposed in the NuGet package; skip it for internal types.

## Testing Conventions

- Framework: xUnit + FluentAssertions (both globally imported).
- Test naming: `Having<setup>_When<action>_Then<result>`.
- One test file per public method (including constructors); group files for a class in a directory named after the class (e.g., `StatementDocument/LoadFromFileAsyncTests.cs`).
- `Assert.Throws` lambdas always use block bodies (`{ }`), not expression bodies.
- Embedded CSV test resources live in `<TestClass>.resources/` directories and are accessed via `TestResources.GetEmbeddedResourceAsText(FileExtension.Csv)` — the helper resolves the resource name from the calling method name by convention.
