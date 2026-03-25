namespace Application.Common.Exceptions;

public sealed class EntityNotFoundException : Exception
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
