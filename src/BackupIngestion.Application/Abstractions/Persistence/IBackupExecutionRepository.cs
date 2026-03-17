using BackupIngestion.Application.DTOs;
using BackupIngestion.Domain.Entities;

namespace BackupIngestion.Application.Abstractions.Persistence;

public interface IBackupExecutionRepository
{
  Task AddRangeAsync(IEnumerable<BackupExecution> executions, CancellationToken cancellationToken = default);
  Task<PagedResultDto<BackupExecution>> SearchAsync(
      BackupSearchParamsDto searchParams,
      CancellationToken cancellationToken = default);
  Task<BackupExecution?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
