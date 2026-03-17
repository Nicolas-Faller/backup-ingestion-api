using BackupIngestion.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackupIngestion.Infrastructure.Persistence;

public class BackupDbContext : DbContext
{
    public BackupDbContext(DbContextOptions<BackupDbContext> options)
        : base(options)
    {
    }

    public DbSet<BackupExecution> BackupExecutions => Set<BackupExecution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BackupDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
