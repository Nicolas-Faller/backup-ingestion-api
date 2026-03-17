using BackupIngestion.Application.Abstractions.Persistence;
using BackupIngestion.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackupIngestion.Infrastructure.Persistence.Repositories;

public class BackupExecutionRepository : IBackupExecutionRepository
{
  private readonly BackupDbContext _context;

  public BackupExecutionRepository(BackupDbContext context)
  {
    _context = context;
  }

  public async Task AddRangeAsync(IEnumerable<BackupExecution> executions, CancellationToken cancellationToken = default)
  {
    await _context.BackupExecutions.AddRangeAsync(executions, cancellationToken);
  }

  public async Task<IReadOnlyList<BackupExecution>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    return await _context.BackupExecutions
        .AsNoTracking()
        .OrderByDescending(x => x.StartedAtUtc)
        .ToListAsync(cancellationToken);
  }

  public async Task<BackupExecution?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    return await _context.BackupExecutions
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
  }

  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return _context.SaveChangesAsync(cancellationToken);
  }
}
