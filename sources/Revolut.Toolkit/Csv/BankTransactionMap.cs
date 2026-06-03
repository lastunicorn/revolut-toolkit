using CsvHelper.Configuration;

namespace DustInTheWind.Revolut.Toolkit.Csv;

internal sealed class BankTransactionMap : ClassMap<BankTransaction>
{
    public BankTransactionMap()
    {
        Map(x => x.Type)
            .Name("Type")
            .TypeConverter<TransactionTypeConverter>();

        Map(x => x.Product)
            .Name("Product")
            .TypeConverter<TransactionProductConverter>();

        Map(x => x.StartedDate)
            .Name("Started Date")
            .TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");

        Map(x => x.CompletedDate)
            .Name("Completed Date")
            .TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");

        Map(x => x.Description).Name("Description");

        Map(x => x.Amount).Name("Amount");

        Map(x => x.Fee).Name("Fee");

        Map(x => x.Currency)
            .Name("Currency")
            .TypeConverter<TransactionCurrencyConverter>();

        Map(x => x.State).Name("State");

        Map(x => x.Balance).Name("Balance");
    }
}