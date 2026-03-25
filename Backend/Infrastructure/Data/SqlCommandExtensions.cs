using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Data
{
    public static class SqlCommandExtensions
    {
        public static SqlParameter AddInParam(this IDbCommand cmd, string name, object? value,
                                                SqlDbType? dbType = null, int? size = null) {
            var sqlCmd = (SqlCommand)cmd;
            var p = sqlCmd.Parameters.Add(new SqlParameter
            {
                ParameterName = name.StartsWith("@") ? name : "@" + name,
                Value = value ?? DBNull.Value
            });

            if (dbType.HasValue) p.SqlDbType = dbType.Value;
            if (size.HasValue) p.Size = size.Value;

            p.Direction = ParameterDirection.Input;
            return p;
        }

        public static SqlParameter AddOutParam(this IDbCommand cmd, string name, SqlDbType dbType, int size = 0)
        {
            var sqlCmd = (SqlCommand)cmd;
            var p = sqlCmd.Parameters.Add(new SqlParameter
            {
                ParameterName = name.StartsWith("@") ? name : "@" + name,
                SqlDbType = dbType,
                Direction = ParameterDirection.Output,
                Size = size
            });
            return p;
        }
    }
}
