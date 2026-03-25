using System.Data.Common;


namespace Infrastructure.Data
{
    public static class DataReaderExtensions
    {
        public static T Get<T>(this DbDataReader rdr, string column)
        {
            var ordinal = rdr.GetOrdinal(column);
            if (rdr.IsDBNull(ordinal))
            {
                if (default(T) is null) return default!;
                throw new InvalidOperationException($"Column '{column}' is NULL but requested as non-nullable {typeof(T).Name}.");
            }

            object value = rdr.GetValue(ordinal);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static T? GetNullable<T>(this DbDataReader rdr, string column) where T : struct
        {
            var ordinal = rdr.GetOrdinal(column);
            if (rdr.IsDBNull(ordinal)) return null;
            object value = rdr.GetValue(ordinal);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static string? GetStringOrNull(this DbDataReader rdr, string column)
        {
            var ordinal = rdr.GetOrdinal(column);
            return rdr.IsDBNull(ordinal) ? null : rdr.GetString(ordinal);
        }
    }
}
