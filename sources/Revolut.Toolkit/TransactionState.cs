namespace DustInTheWind.Revolut.Toolkit;

/// <summary>
/// Value object that represents a transaction state.
/// </summary>
public sealed record class TransactionState
{
	public static readonly TransactionState Completed = new("COMPLETED");

	public static readonly IReadOnlyCollection<TransactionState> KnownValues =
	[
		Completed
	];

	public string Value { get; }

	public TransactionState(string value)
	{
		Value = value ?? throw new ArgumentNullException(nameof(value));
	}

	public override string ToString()
	{
		return Value;
	}

	public static implicit operator TransactionState(string value)
	{
		return value == null
			? null
			: new TransactionState(value);
	}

	public static implicit operator string(TransactionState transactionState)
	{
		return transactionState?.Value;
	}
}