using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using MissingFieldException = CsvHelper.MissingFieldException;

namespace DustInTheWind.Revolut.Toolkit.Csv;

internal class CsvStatementDocument
{
	private readonly CsvReader csvReader;
	private bool classMapRegistered;
	private CsvDocumentReadState state;

	public CsvStatementDocument(TextReader textReader)
	{
		if (textReader == null) throw new ArgumentNullException(nameof(textReader));

		CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = true,
			IgnoreBlankLines = true,
			TrimOptions = TrimOptions.Trim,
			PrepareHeaderForMatch = args => args.Header.Trim()
		};

		csvReader = new CsvReader(textReader, csvConfiguration);
	}

	public string[] ReadHeaders()
	{
		if (state != CsvDocumentReadState.HeaderRow)
			throw new InvalidOperationException("Header row was already read.");

		try
		{
			csvReader.Read();
			csvReader.ReadHeader();

			state = CsvDocumentReadState.DataRow;
			return csvReader.HeaderRecord ?? [];
		}
		catch (HeaderValidationException ex)
		{
			throw new HeaderLoadException(ex);
		}
		catch (ReaderException ex) when (ex is MissingFieldException || ex.InnerException is HeaderValidationException or MissingFieldException)
		{
			throw new HeaderLoadException(ex.InnerException ?? ex);
		}
		catch (ReaderException ex)
		{
			throw new DataLoadException(ex);
		}
		catch (TypeConverterException ex)
		{
			throw new DataLoadException(ex);
		}
		catch (Exception ex)
		{
			throw new DocumentLoadException(ex);
		}
	}

	public async IAsyncEnumerable<BankTransaction> ReadTransactions()
	{
		if (state == CsvDocumentReadState.HeaderRow)
			_ = ReadHeaders();

		if (state == CsvDocumentReadState.Ended)
			yield break;

		if (state != CsvDocumentReadState.DataRow)
			throw new InvalidOperationException("CSV document is not in a valid state to read transactions.");

		if (!classMapRegistered)
		{
			csvReader.Context.RegisterClassMap(new BankTransactionMap());
			classMapRegistered = true;
		}

		while (true)
		{
			bool hasRecord;

			try
			{
				hasRecord = await csvReader.ReadAsync();
			}
			catch (HeaderValidationException ex)
			{
				throw new HeaderLoadException(ex);
			}
			catch (ReaderException ex) when (ex is MissingFieldException || ex.InnerException is HeaderValidationException or MissingFieldException)
			{
				throw new HeaderLoadException(ex.InnerException ?? ex);
			}
			catch (ReaderException ex)
			{
				throw new DataLoadException(ex);
			}
			catch (TypeConverterException ex)
			{
				throw new DataLoadException(ex);
			}
			catch (Exception ex)
			{
				throw new DocumentLoadException(ex);
			}

			if (!hasRecord)
			{
				state = CsvDocumentReadState.Ended;
				yield break;
			}

			BankTransaction transaction;

			try
			{
				transaction = csvReader.GetRecord<BankTransaction>();
			}
			catch (HeaderValidationException ex)
			{
				throw new HeaderLoadException(ex);
			}
			catch (ReaderException ex) when (ex is MissingFieldException || ex.InnerException is HeaderValidationException or MissingFieldException)
			{
				throw new HeaderLoadException(ex.InnerException ?? ex);
			}
			catch (ReaderException ex)
			{
				throw new DataLoadException(ex);
			}
			catch (TypeConverterException ex)
			{
				throw new DataLoadException(ex);
			}
			catch (Exception ex)
			{
				throw new DocumentLoadException(ex);
			}

			yield return transaction;
		}
	}
}