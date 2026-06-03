using DustInTheWind.Revolut.Toolkit.Csv;

namespace DustInTheWind.Revolut.Toolkit;

/// <summary>
/// Contains a list of bank transactions. It is rendered as a csv file.
/// </summary>
public class TransactionsDocument
{
	public List<BankTransaction> Transactions { get; } = [];

	public static Task<TransactionsDocument> LoadFromFileAsync(string filePath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		try
		{
			using StreamReader streamReader = File.OpenText(filePath);
			return LoadInternalAsync(streamReader);
		}
		catch (DocumentLoadException)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw new DocumentLoadException(ex);
		}
	}

	public static Task<TransactionsDocument> LoadAsync(string csv)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(csv);

		try
		{
			using StringReader stringReader = new(csv);
			return LoadInternalAsync(stringReader);
		}
		catch (DocumentLoadException)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw new DocumentLoadException(ex);
		}
	}

	public static Task<TransactionsDocument> LoadAsync(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		try
		{
			using StreamReader streamReader = new(stream);
			return LoadInternalAsync(streamReader);
		}
		catch (DocumentLoadException)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw new DocumentLoadException(ex);
		}
	}

	public static Task<TransactionsDocument> LoadAsync(FileInfo fileInfo)
	{
		ArgumentNullException.ThrowIfNull(fileInfo);

		try
		{
			using StreamReader streamReader = fileInfo.OpenText();
			return LoadInternalAsync(streamReader);
		}
		catch (DocumentLoadException)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw new DocumentLoadException(ex);
		}
	}

	public static Task<TransactionsDocument> LoadAsync(StreamReader streamReader)
	{
		ArgumentNullException.ThrowIfNull(streamReader);

		return LoadInternalAsync(streamReader);
	}

	public static Task<TransactionsDocument> LoadAsync(TextReader textReader)
	{
		ArgumentNullException.ThrowIfNull(textReader);

		return LoadInternalAsync(textReader);
	}

	private static async Task<TransactionsDocument> LoadInternalAsync(TextReader textReader)
	{
		TransactionsCsvDocument transactionsCsvDocument = new(textReader);
		TransactionsDocument transactionsDocument = new();

		await foreach (BankTransaction bankTransaction in transactionsCsvDocument.ReadTransactions())
			transactionsDocument.Transactions.Add(bankTransaction);

		return transactionsDocument;
	}
}