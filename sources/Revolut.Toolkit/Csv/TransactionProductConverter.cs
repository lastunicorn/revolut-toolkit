using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DustInTheWind.Revolut.Toolkit.Csv;

internal sealed class TransactionProductConverter : DefaultTypeConverter
{
	public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
	{
		if (string.IsNullOrWhiteSpace(text))
			throw new TypeConverterException(this, memberMapData, text, row.Context, "Transaction product cannot be empty.");

		try
		{
			return new TransactionProduct(text);
		}
		catch (ArgumentException ex)
		{
			throw new TypeConverterException(this, memberMapData, text, row.Context, ex.Message, ex);
		}
	}

	public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
	{
		return value is TransactionProduct transactionProduct
			? transactionProduct.Value
			: base.ConvertToString(value, row, memberMapData);
	}
}