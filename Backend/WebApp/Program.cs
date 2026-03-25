using Application.DI;
using Infrastructure.DI;
using Infrastructure.Initialization;
using WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOriginPolicy",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

await app.Services.InitializeInfrastructureDatabaseAsync();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowAnyOriginPolicy");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
