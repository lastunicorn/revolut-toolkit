using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DustInTheWind.Revolut.Toolkit.Csv;

internal sealed class TransactionStateConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new TypeConverterException(this, memberMapData, text, row.Context, "Transaction state cannot be empty.");

        try
        {
            return new TransactionState(text);
        }
        catch (ArgumentException ex)
        {
            throw new TypeConverterException(this, memberMapData, text, row.Context, ex.Message, ex);
        }
    }

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        return value is TransactionState transactionState
            ? transactionState.Value
            : base.ConvertToString(value, row, memberMapData);
    }
}

