namespace DustInTheWind.Revolut.Toolkit;

public class DocumentLoadException : Exception
{
    private const string DefaultMessage = "The transactions document is invalid.";

    public DocumentLoadException()
        : base(DefaultMessage)
    {
    }

    public DocumentLoadException(Exception innerException)
        : base(DefaultMessage, innerException)
    {
    }

    public DocumentLoadException(string message)
        : base(message)
    {
    }

    public DocumentLoadException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}