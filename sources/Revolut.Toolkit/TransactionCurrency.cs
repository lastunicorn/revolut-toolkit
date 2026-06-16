namespace DustInTheWind.Revolut.Toolkit;

/// <summary>
/// Value object that represents a transaction currency (ISO 4217 currency code).
/// </summary>
public sealed record class TransactionCurrency
{
	public static readonly TransactionCurrency RON = new("RON");
	public static readonly TransactionCurrency EUR = new("EUR");
	public static readonly TransactionCurrency USD = new("USD");
	public static readonly TransactionCurrency GBP = new("GBP");

	public static readonly IReadOnlyCollection<TransactionCurrency> KnownValues =
	[
		RON,
		EUR,
		USD,
		GBP
	];

	public string Value { get; }

	public TransactionCurrency(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentNullException(nameof(value));

		if (value.Length != 3)
			throw new ArgumentException("Currency code must be exactly 3 characters long.", nameof(value));

		Value = value.ToUpperInvariant();
	}

	public override string ToString()
	{
		return Value;
	}

	public static implicit operator TransactionCurrency(string value)
	{
		return value == null
			? null
			: new TransactionCurrency(value);
	}

	public static implicit operator string(TransactionCurrency transactionCurrency)
	{
		return transactionCurrency?.Value;
	}
}