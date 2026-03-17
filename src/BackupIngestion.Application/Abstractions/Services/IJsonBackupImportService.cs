using BackupIngestion.Application.DTOs;

namespace BackupIngestion.Application.Abstractions.Services;

public interface IJsonBackupImportService
{
  Task<ImportJsonResultDto> ImportAsync(string jsonContent, CancellationToken cancellationToken = default);
}
