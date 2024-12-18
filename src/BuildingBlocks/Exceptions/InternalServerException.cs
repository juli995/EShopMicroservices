namespace BuildingBlocks.Exceptions;

public sealed class InternalServerException : Exception
{
    public string? Details { get; set; }

    public InternalServerException(string message) : base(message)
    { }

    public InternalServerException(string message, string details)
    {
        Details = details;
    }
}