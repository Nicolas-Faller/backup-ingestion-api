using BackupIngestion.Domain.Entities;

namespace BackupIngestion.Application.Abstractions.Persistence;

public interface IBackupExecutionRepository
{
  Task AddRangeAsync(IEnumerable<BackupExecution> executions, CancellationToken cancellationToken = default);
  Task<IReadOnlyList<BackupExecution>> GetAllAsync(CancellationToken cancellationToken = default);
  Task<BackupExecution?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
