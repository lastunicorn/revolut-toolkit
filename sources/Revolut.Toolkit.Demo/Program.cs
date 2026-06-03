using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.Revolut.Toolkit.Demo;

internal static class Program
{
	public static async Task Main(string[] args)
	{
		const string fileName = "transactions.csv";

		try
		{
			TransactionsDocument document = await TransactionsDocument.LoadFromFileAsync(fileName);

			DataGrid dataGrid = Display(document);
			dataGrid.Display();
		}
		catch (DocumentLoadException ex)
		{
			await Console.Error.WriteLineAsync($"Failed to read '{fileName}': {ex.Message}");
			Environment.ExitCode = 1;
		}
		catch (Exception ex)
		{
			await Console.Error.WriteLineAsync($"Unexpected error: {ex.Message}");
			Environment.ExitCode = 1;
		}
	}

	private static DataGrid Display(TransactionsDocument document)
	{
		DataGrid dataGrid = new()
		{
			Title = $"Transactions",
			BorderTemplate = BorderTemplate.PlusMinusBorderTemplate,
			Footer = $"Count: {document.Transactions.Count}"
		};

		dataGrid.Columns.Add("Type");
		dataGrid.Columns.Add("Product");
		dataGrid.Columns.Add("StartedDate");
		dataGrid.Columns.Add("CompletedDate");
		dataGrid.Columns.Add("Description");
		dataGrid.Columns.Add("Amount", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Fee", HorizontalAlignment.Right);
		dataGrid.Columns.Add("Currency", HorizontalAlignment.Right);
		dataGrid.Columns.Add("State");
		dataGrid.Columns.Add("Balance", HorizontalAlignment.Right);

		foreach (BankTransaction transaction in document.Transactions)
		{
			dataGrid.Rows.Add(
				transaction.Type,
				transaction.Product,
				transaction.StartedDate.ToString("yyyy-MM-dd HH:mm:ss"),
				transaction.CompletedDate.ToString("yyyy-MM-dd HH:mm:ss"),
				transaction.Description,
				transaction.Amount.ToString(),
				transaction.Fee.ToString(),
				transaction.Currency,
				transaction.State,
				transaction.Balance);
		}

		return dataGrid;
	}
}