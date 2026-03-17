using BackupIngestion.Application.Abstractions.Persistence;
using BackupIngestion.Application.DTOs;
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

  public async Task<PagedResultDto<BackupExecution>> SearchAsync(
      BackupSearchParamsDto searchParams,
      CancellationToken cancellationToken = default)
  {
    var query = _context.BackupExecutions
        .AsNoTracking()
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchParams.ClientName))
    {
      var clientName = searchParams.ClientName.Trim();
      query = query.Where(x => x.ClientName.Contains(clientName));
    }

    if (searchParams.Status.HasValue)
    {
      query = query.Where(x => x.Status == searchParams.Status.Value);
    }

    if (searchParams.SourceType.HasValue)
    {
      query = query.Where(x => x.SourceType == searchParams.SourceType.Value);
    }

    if (searchParams.StartDate.HasValue)
    {
      query = query.Where(x => x.StartedAtUtc >= searchParams.StartDate.Value);
    }

    if (searchParams.EndDate.HasValue)
    {
      query = query.Where(x => x.StartedAtUtc <= searchParams.EndDate.Value);
    }

    var totalCount = await query.CountAsync(cancellationToken);

    var items = await query
        .OrderByDescending(x => x.StartedAtUtc)
        .Skip((searchParams.Page - 1) * searchParams.PageSize)
        .Take(searchParams.PageSize)
        .ToListAsync(cancellationToken);

    return new PagedResultDto<BackupExecution>(
        items,
        totalCount,
        searchParams.Page,
        searchParams.PageSize);
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
