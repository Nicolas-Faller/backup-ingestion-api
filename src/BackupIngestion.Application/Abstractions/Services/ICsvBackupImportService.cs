using BackupIngestion.Application.DTOs;

namespace BackupIngestion.Application.Abstractions.Services;

public interface ICsvBackupImportService
{
  Task<ImportCsvResultDto> ImportAsync(string csvContent, CancellationToken cancellationToken = default);
}
