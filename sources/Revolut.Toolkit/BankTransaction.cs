namespace DustInTheWind.Revolut.Toolkit;

/// <summary>
/// Represents a bank transaction from Revolut Bank.
/// </summary>
public record class BankTransaction
{
    public string Type { get; set; }

    public string Product { get; set; }

    public DateTime StartedDate { get; set; }

    public DateTime CompletedDate { get; set; }

    public string Description { get; set; }

    public decimal Amount { get; set; }

    public decimal Fee { get; set; }

    public string Currency { get; set; }

    public string State { get; set; }

    public decimal Balance { get; set; }
}