using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Infrastructure.Initialization;

public sealed class SqlDatabaseInitializer : IDatabaseInitializer
{
    private static readonly Regex GoRegex = new(@"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<SqlDatabaseInitializer> _logger;
    private readonly DatabaseInitializationOptions _options;

    public SqlDatabaseInitializer(
        ISqlConnectionFactory connectionFactory,
        IHostEnvironment environment,
        IOptions<DatabaseInitializationOptions> options,
        ILogger<SqlDatabaseInitializer> logger)
    {
        _connectionFactory = connectionFactory;
        _environment = environment;
        _logger = logger;
        _options = options.Value;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation("Database initialization is disabled.");
            return;
        }

        var scriptsRoot = ResolveScriptsRoot();
        var scriptFiles = GetOrderedScriptFiles(scriptsRoot);

        await using var connection = (SqlConnection)_connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        foreach (var scriptFile in scriptFiles)
        {
            var script = await File.ReadAllTextAsync(scriptFile, cancellationToken);
            var batches = SplitBatches(script);

            foreach (var batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch))
                {
                    continue;
                }

                await using var command = connection.CreateCommand();
                command.CommandText = batch;
                command.CommandTimeout = 120;
                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            _logger.LogInformation("Executed SQL script: {ScriptFile}", Path.GetFileName(scriptFile));
        }
    }

    private string ResolveScriptsRoot()
    {
        var candidatePaths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Scripts"),
            Path.Combine(_environment.ContentRootPath, "Scripts"),
            Path.Combine(_environment.ContentRootPath, "..", "Infrastructure", "Scripts")
        };

        foreach (var scriptsRoot in candidatePaths.Select(Path.GetFullPath).Distinct())
        {
            if (Directory.Exists(scriptsRoot))
            {
                return scriptsRoot;
            }
        }

        throw new DirectoryNotFoundException("Scripts directory was not found in any of the expected locations.");
    }

    private IEnumerable<string> GetOrderedScriptFiles(string scriptsRoot)
    {
        var orderedFiles = new List<string>();

        if (_options.ResetOnStartup)
        {
            orderedFiles.Add(Path.Combine(scriptsRoot, "Tables", "tables.sql"));
        }

        orderedFiles.AddRange(
            Directory.GetFiles(Path.Combine(scriptsRoot, "StoredProcedures"), "*.sql")
                .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase));

        return orderedFiles;
    }

    private static IEnumerable<string> SplitBatches(string script)
    {
        return GoRegex.Split(script)
            .Select(batch => batch.Trim())
            .Where(batch => !string.IsNullOrWhiteSpace(batch));
    }
}
