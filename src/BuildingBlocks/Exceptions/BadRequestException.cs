namespace BuildingBlocks.Exceptions;

public sealed class BadRequestException : Exception
{
    public string? Details { get; set; }

    public BadRequestException(string message) : base(message)
    { }

    public BadRequestException(string message, string details)
    {
        Details = details;
    }
}