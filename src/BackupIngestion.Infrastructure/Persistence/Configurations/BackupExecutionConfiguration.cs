using BackupIngestion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackupIngestion.Infrastructure.Persistence.Configurations;

public class BackupExecutionConfiguration : IEntityTypeConfiguration<BackupExecution>
{
  public void Configure(EntityTypeBuilder<BackupExecution> builder)
  {
    builder.ToTable("BackupExecutions");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.ClientName)
        .IsRequired()
        .HasMaxLength(150);

    builder.Property(x => x.JobName)
        .IsRequired()
        .HasMaxLength(200);

    builder.Property(x => x.SourceType)
        .IsRequired()
        .HasConversion<int>();

    builder.Property(x => x.Status)
        .IsRequired()
        .HasConversion<int>();

    builder.Property(x => x.StartedAtUtc)
        .IsRequired();

    builder.Property(x => x.FinishedAtUtc);

    builder.Property(x => x.DurationSeconds)
        .IsRequired();

    builder.Property(x => x.DataSizeBytes)
        .IsRequired();

    builder.Property(x => x.BackupSizeBytes)
        .IsRequired();

    builder.Property(x => x.TransferredSizeBytes)
        .IsRequired();

    builder.Property(x => x.Message)
        .HasMaxLength(1000);

    builder.Property(x => x.ImportedAtUtc)
        .IsRequired();

    builder.HasIndex(x => x.ClientName);
    builder.HasIndex(x => x.Status);
    builder.HasIndex(x => x.SourceType);
    builder.HasIndex(x => x.StartedAtUtc);
  }
}
