using Domain.AggregateModels.Invoice;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Infrastructure.Persistence.Invoices
{
    public class SqlInvoicesWriter : IInvoiceWriter
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public SqlInvoicesWriter(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Invoice> CreateAsync(Invoice invoiceData, List<InvoiceDetail> invoiceDetails, CancellationToken cancellationToken = default)
        {
            try
            {
                var invoiceItemsDetailTable = new DataTable();
                invoiceItemsDetailTable.Columns.Add("IdProducto", typeof(int));
                invoiceItemsDetailTable.Columns.Add("CantidadDelProducto", typeof(int));
                invoiceItemsDetailTable.Columns.Add("PrecioUnitarioProducto", typeof(decimal));
                invoiceItemsDetailTable.Columns.Add("SubtotalProducto", typeof(decimal));
                invoiceItemsDetailTable.Columns.Add("Notas", typeof(string));

                foreach (var it in invoiceDetails)
                {
                    invoiceItemsDetailTable.Rows.Add(
                        it.ProductId,
                        it.ProductQuantity,
                        it.ProductUnitPrice,
                        it.Subtotal,
                        (object?)it.Note ?? DBNull.Value
                    );
                }

                await using var conn = (SqlConnection)_connectionFactory.CreateConnection();
                await conn.OpenAsync(cancellationToken);

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.sp_Invoice_Create";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddInParam("FechaEmisionFactura", invoiceData.DateTime, SqlDbType.DateTime);
                cmd.AddInParam("IdCliente", invoiceData.ClientId, SqlDbType.Int);
                cmd.AddInParam("NumeroFactura", invoiceData.InvoiceNumber, SqlDbType.Int);
                cmd.AddInParam("NumeroTotalArticulos", invoiceData.ItemsCount, SqlDbType.Int);

                cmd.AddInParam("SubTotalFactura", invoiceData.Subtotal, SqlDbType.Decimal);
                cmd.Parameters["@SubTotalFactura"].Precision = 12;
                cmd.Parameters["@SubTotalFactura"].Scale = 2;

                cmd.AddInParam("TotalImpuesto", invoiceData.Tax, SqlDbType.Decimal);
                cmd.Parameters["@TotalImpuesto"].Precision = 12;
                cmd.Parameters["@TotalImpuesto"].Scale = 2;

                cmd.AddInParam("TotalFactura", invoiceData.Total, SqlDbType.Decimal);
                cmd.Parameters["@TotalFactura"].Precision = 12;
                cmd.Parameters["@TotalFactura"].Scale = 2;

                var items = cmd.Parameters.AddWithValue("@Items", invoiceItemsDetailTable);
                items.SqlDbType = SqlDbType.Structured;
                items.TypeName = "dbo.InvoiceItemTVP";

                var newId = cmd.AddOutParam("NewId", SqlDbType.Int);

                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                var r = (DbDataReader)reader;

                if (!await r.ReadAsync(cancellationToken))
                    throw new InvalidOperationException("");

                var invoiceCreated = new Invoice(r.Get<int>("IdFactura"), r.Get<int>("NumeroFactura"), r.Get<int>("IdCliente"), r.Get<int>("NumeroTotalArticulos"),
                                          r.Get<DateTime>("FechaEmisionFactura"), r.Get<decimal>("SubTotalFactura"), r.Get<decimal>("TotalImpuesto"),
                                          r.Get<decimal>("TotalFactura"));

                return invoiceCreated;
            }
            catch (SqlException ex)
            {
                throw SqlExceptionHandler.HandleException(ex);
            }
        }
    }
}
