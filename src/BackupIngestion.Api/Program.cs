using BackupIngestion.Application.Abstractions.Services;
using BackupIngestion.Application.Services;
using BackupIngestion.Infrastructure.DependencyInjection;
using BackupIngestion.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IJsonBackupImportService, JsonBackupImportService>();
builder.Services.AddScoped<ICsvBackupImportService, CsvBackupImportService>();
builder.Services.AddScoped<IHtmlBackupImportService, HtmlBackupImportService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<BackupDbContext>();
  await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
