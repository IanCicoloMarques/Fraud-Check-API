
using FraudCheckAPI.Data.Sql.Interfaces;
using FraudCheckAPI.Data.Sql.Repositories;
using FraudCheckAPI.Interfaces;
using FraudCheckAPI.Services;
using FraudCheckAPI.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddSingleton<IFraudCheckRepository, FraudCheckRepository>();
builder.Services.AddSingleton<IAnalysisService, AnalysisService>();


builder.Services.AddOptions<DatabaseSettings>()
    .Bind(builder.Configuration.GetSection("ConnectionStrings"));


builder.Services.AddOptions<AnalysisApiSettings>()
    .Bind(builder.Configuration.GetSection("Url"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
