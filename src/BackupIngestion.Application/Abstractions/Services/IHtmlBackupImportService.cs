using BackupIngestion.Application.DTOs;

namespace BackupIngestion.Application.Abstractions.Services;

public interface IHtmlBackupImportService
{
  Task<ImportHtmlResultDto> ImportAsync(string htmlContent, CancellationToken cancellationToken = default);
}
