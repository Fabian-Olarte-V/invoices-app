namespace Application.Common.Exceptions;

public sealed class ApplicationValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ApplicationValidationException(IDictionary<string, string[]> errors)
        : base("Se encontraron uno o mas errores de validacion")
    {
        Errors = errors;
    }
}
