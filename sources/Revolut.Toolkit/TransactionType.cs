namespace DustInTheWind.Revolut.Toolkit;

/// <summary>
/// Value object that represents supported transaction types.
/// </summary>
public sealed record class TransactionType
{
	public static readonly TransactionType Interest = new("Interest");
	public static readonly TransactionType Transfer = new("Transfer");
	public static readonly TransactionType Deposit = new("Deposit");

	public static readonly IReadOnlyCollection<TransactionType> KnownValues =
	[
		Interest,
		Transfer,
		Deposit
	];

	public string Value { get; }

	public TransactionType(string value)
	{
		Value = value ?? throw new ArgumentNullException(nameof(value));
	}

	public override string ToString()
	{
		return Value;
	}

	public static implicit operator TransactionType(string value)
	{
		return value == null
			? null
			: new TransactionType(value);
	}

	public static implicit operator string(TransactionType transactionType)
	{
		return transactionType?.Value;
	}
}