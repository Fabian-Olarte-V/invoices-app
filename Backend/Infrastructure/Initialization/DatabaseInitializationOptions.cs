namespace Infrastructure.Initialization;

public sealed class DatabaseInitializationOptions
{
    public const string SectionName = "DatabaseInitialization";

    public bool Enabled { get; set; }
    public bool ResetOnStartup { get; set; } = true;
}
