using Domain.AggregateModels.Client;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Infrastructure.Persistence.Clients
{
    public class SqlClientsQueries : IClientFinder
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public SqlClientsQueries(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await using var conn = (SqlConnection)_connectionFactory.CreateConnection();
                await conn.OpenAsync(cancellationToken);

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.sp_Client_List";
                cmd.CommandType = CommandType.StoredProcedure;

                var list = new List<Client>();

                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                var r = (DbDataReader)reader;

                while (await r.ReadAsync(cancellationToken))
                {
                    list.Add(new Client(r.Get<int>("Id"),
                                      r.Get<string>("RazonSocial"),
                                      r.Get<int>("IdTipoCliente"),
                                      r.Get<DateTime>("FechaCreacion"),
                                      r.Get<string>("RFC"))
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
