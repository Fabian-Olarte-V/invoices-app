using Infrastructure.Exceptions;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

public static class SqlExceptionHandler
{
    public static Exception HandleException(SqlException ex)
    {
        if(ex.Message.Contains("Violation of UNIQUE KEY"))
        {
            throw new UniqueConstraintException("El registro ya existe en la base de datos", ex.Message);
        }
        else if(ex.Message.Contains("FOREIGN KEY constraint"))
        {
            throw new ForeignKeyConstraintException("El registro tiene un conflicto con una relacion FK", ex.Message);
        }
        else
        {
            throw ex;
        }
    }
}
