using BackupIngestion.Domain.Entities;

namespace BackupIngestion.Application.DTOs;

public sealed record BackupExecutionDto(
    Guid Id,
    string ClientName,
    string JobName,
    string SourceType,
    string Status,
    DateTime StartedAtUtc,
    DateTime? FinishedAtUtc,
    int DurationSeconds,
    long DataSizeBytes,
    long BackupSizeBytes,
    long TransferredSizeBytes,
    string? Message,
    DateTime ImportedAtUtc)
{
  public static BackupExecutionDto FromEntity(BackupExecution entity) =>
      new(
          entity.Id,
          entity.ClientName,
          entity.JobName,
          entity.SourceType.ToString(),
          entity.Status.ToString(),
          entity.StartedAtUtc,
          entity.FinishedAtUtc,
          entity.DurationSeconds,
          entity.DataSizeBytes,
          entity.BackupSizeBytes,
          entity.TransferredSizeBytes,
          entity.Message,
          entity.ImportedAtUtc);
}
