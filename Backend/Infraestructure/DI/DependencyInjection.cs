using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infraestructure.Data;
using Infraestructure.Persistance.Clients;
using Infraestructure.Persistance.Invoices;
using Infraestructure.Persistance.Products;
using Domain.AggregateModels.Client;
using Domain.AggregateModels.Product;
using Domain.AggregateModels.Invoice;


namespace Infraestructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration config)
        {
            var cs = config.GetConnectionString("database") ?? throw new InvalidOperationException("Missing connection string");
            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(cs));

            services.AddScoped<IClientFinder, SqlClientsQueries>();
            services.AddScoped<IProductFinder, SqlProductsQueries>();
            services.AddScoped<IInvoiceFinder, SqlInvoicesQueries>();
            services.AddScoped<IInvoiceWriter, SqlInvoicesWriter>();

            return services;
        }   
    }
}
