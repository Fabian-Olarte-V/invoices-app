namespace Infrastructure.Exceptions;

public sealed class UniqueConstraintException : Exception
{
    public string Field { get; }

    public UniqueConstraintException(string field, string message)
        : base(message)
    {
        Field = field;
    }
}
