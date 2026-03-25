using Domain.AggregateModels.Product;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Infrastructure.Persistence.Products
{
    public class SqlProductsQueries : IProductFinder
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public SqlProductsQueries(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await using var conn = (SqlConnection)_connectionFactory.CreateConnection();
                await conn.OpenAsync(cancellationToken);

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.sp_Product_List";
                cmd.CommandType = CommandType.StoredProcedure;

                var list = new List<Product>();

                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                var r = (DbDataReader)reader;

                while (await r.ReadAsync(cancellationToken))
                {
                    byte[]? img = null;
                    var ordinal = r.GetOrdinal("ImagenProducto");
                    if (!r.IsDBNull(ordinal)) img = (byte[])r.GetValue(ordinal);

                    list.Add(new Product(r.Get<int>("Id"), r.Get<string>("NombreProducto"),
                                        r.Get<decimal>("PrecioUnitario"), img, r.GetStringOrNull("ext"))
                        );
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionHandler.HandleException(ex);
            }
        }
    }
}
