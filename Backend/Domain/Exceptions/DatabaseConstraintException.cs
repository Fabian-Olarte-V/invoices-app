namespace Domain.Exceptions;

public class UniqueConstraintException : DomainException
{
    public string Field { get; }

    public UniqueConstraintException(string field, string message) 
        : base(message)
    {
        Field = field;
    }
}

public class ForeingKeyConstraintException : DomainException
{
    public string Field { get; }

    public ForeingKeyConstraintException(string field, string message)
        : base(message)
    {
        Field = field;
    }
}

