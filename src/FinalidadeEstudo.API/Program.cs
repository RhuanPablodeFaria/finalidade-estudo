using FinalidadeEstudo.CrossCutting.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddLoggingServices(builder.Configuration)
                .AddVersioningServices()
                .AddApplicationServices()
                .AddDependencyInjection();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
