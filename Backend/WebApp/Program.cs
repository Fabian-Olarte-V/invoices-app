using Application.DI;
using Infraestructure.DI;
using WebApp.Filters;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
builder.Services.AddApplication();
builder.Services.AddInfraestructure(builder.Configuration);

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


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseCors("AllowAnyOriginPolicy");

app.UseRouting();
app.UseAuthorization();
app.MapControllers();



app.Run();
