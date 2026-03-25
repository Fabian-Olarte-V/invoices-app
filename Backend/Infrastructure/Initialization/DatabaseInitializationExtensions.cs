using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Initialization;

public static class DatabaseInitializationExtensions
{
    public static async Task InitializeInfrastructureDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
        await initializer.InitializeAsync(cancellationToken);
    }
}
