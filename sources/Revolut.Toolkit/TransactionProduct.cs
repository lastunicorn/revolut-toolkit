namespace DustInTheWind.Revolut.Toolkit;

/// <summary>
/// Value object that represents supported transaction products.
/// </summary>
public sealed record class TransactionProduct
{
	public static readonly TransactionProduct Current = new("Current");
	public static readonly TransactionProduct Deposit = new("Deposit");

	public static readonly IReadOnlyCollection<TransactionProduct> KnownValues =
	[
		Current,
		Deposit
	];

	public string Value { get; }

	public TransactionProduct(string value)
	{
		Value = value ?? throw new ArgumentNullException(nameof(value));
	}

	public override string ToString()
	{
		return Value;
	}

	public static implicit operator TransactionProduct(string value)
	{
		return value == null
			? null
			: new TransactionProduct(value);
	}

	public static implicit operator string(TransactionProduct transactionProduct)
	{
		return transactionProduct?.Value;
	}
}