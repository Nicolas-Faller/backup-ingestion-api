using BackupIngestion.Application.DTOs;

namespace BackupIngestion.Application.Abstractions.Services;

public interface IBackupExcelExportService
{
  Task<byte[]> ExportAsync(
      BackupSearchParamsDto searchParams,
      CancellationToken cancellationToken = default);
}
