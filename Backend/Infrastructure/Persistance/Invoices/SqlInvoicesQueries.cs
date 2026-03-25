using Domain.AggregateModels.Invoice;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Infrastructure.Persistence.Invoices
{
    public class SqlInvoicesQueries : IInvoiceFinder
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public SqlInvoicesQueries(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Invoice>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var conn = (SqlConnection)_connectionFactory.CreateConnection();
                await conn.OpenAsync(cancellationToken);

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.sp_Invoice_GetByClientId";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddInParam("ClientId", clientId, SqlDbType.Int);

                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                var r = (DbDataReader)reader;

                List<Invoice> results = new List<Invoice>();
                while (await r.ReadAsync(cancellationToken))
                {
                    results.Add(new Invoice(r.Get<int>("Id"), r.Get<int>("NumeroFactura"),
                                            r.Get<int>("IdCliente"), r.Get<int>("NumeroTotalArticulos"),
                                            r.Get<DateTime>("FechaEmisionFactura"), r.Get<decimal>("SubTotalFactura"),
                                            r.Get<decimal>("TotalImpuesto"), r.Get<decimal>("TotalFactura"))
                        );
                }

                return results;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionHandler.HandleException(ex);
            }
        }

        public async Task<Invoice> GetByNumberAsync(int invoiceNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var conn = (SqlConnection)_connectionFactory.CreateConnection();
                await conn.OpenAsync(cancellationToken);

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.sp_Invoice_GetByNumber";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddInParam("NumeroFactura", invoiceNumber, SqlDbType.Int);

                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                var r = (DbDataReader)reader;

                if (await r.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    return new Invoice(r.Get<int>("Id"), r.Get<int>("NumeroFactura"),
                                            r.Get<int>("IdCliente"), r.Get<int>("NumeroTotalArticulos"),
                                            r.Get<DateTime>("FechaEmisionFactura"), r.Get<decimal>("SubTotalFactura"),
                                            r.Get<decimal>("TotalImpuesto"), r.Get<decimal>("TotalFactura"));
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionHandler.HandleException(ex);
            }
        }
    }
}
