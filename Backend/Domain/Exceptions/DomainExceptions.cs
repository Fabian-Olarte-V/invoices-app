namespace Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }
}

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entity)
        : base($"No se encontraron datos de la entidad de: \"{entity}\"")
    {
    }

    public EntityNotFoundException(string entity, object key)
        : base($"La entidad \"{entity}\" ({key}) no fue encontrada.")
    {
    }
}

public class BusinessRuleValidationException : DomainException
{
    public BusinessRuleValidationException(string message) : base(message)
    {
    }
}