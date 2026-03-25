using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Infrastructure.Persistence.Clients;
using Infrastructure.Persistence.Invoices;
using Infrastructure.Persistence.Products;
using Infrastructure.Initialization;
using Domain.AggregateModels.Client;
using Domain.AggregateModels.Product;
using Domain.AggregateModels.Invoice;


namespace Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var cs = config.GetConnectionString("database") ?? throw new InvalidOperationException("Missing connection string");
            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(cs));
            services.Configure<DatabaseInitializationOptions>(config.GetSection(DatabaseInitializationOptions.SectionName));
            services.AddScoped<IDatabaseInitializer, SqlDatabaseInitializer>();

            services.AddScoped<IClientFinder, SqlClientsQueries>();
            services.AddScoped<IProductFinder, SqlProductsQueries>();
            services.AddScoped<IInvoiceFinder, SqlInvoicesQueries>();
            services.AddScoped<IInvoiceWriter, SqlInvoicesWriter>();

            return services;
        }   
    }
}
