using BackupIngestion.Application.Abstractions.Parsing;
using BackupIngestion.Application.Abstractions.Persistence;
using BackupIngestion.Infrastructure.Parsers.Csv;
using BackupIngestion.Infrastructure.Parsers.Html;
using BackupIngestion.Infrastructure.Parsers.Json;
using BackupIngestion.Infrastructure.Persistence;
using BackupIngestion.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackupIngestion.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("DefaultConnection")
                           ?? "Data Source=backup_ingestion.db";

    services.AddDbContext<BackupDbContext>(options =>
        options.UseSqlite(connectionString));

    services.AddScoped<IBackupExecutionRepository, BackupExecutionRepository>();
    services.AddScoped<IJsonBackupParser, JsonBackupParser>();
    services.AddScoped<ICsvBackupParser, CsvBackupParser>();
    services.AddScoped<IHtmlBackupParser, HtmlBackupParser>();

    return services;
  }
}
