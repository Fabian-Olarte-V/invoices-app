namespace Infrastructure.Exceptions;

public sealed class ForeignKeyConstraintException : Exception
{
    public string Field { get; }

    public ForeignKeyConstraintException(string field, string message)
        : base(message)
    {
        Field = field;
    }
}
