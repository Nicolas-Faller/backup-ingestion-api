using BackupIngestion.Domain.Enums;

namespace BackupIngestion.Domain.Entities;

public class BackupExecution
{
  public Guid Id { get; private set; } = Guid.NewGuid();
  public string ClientName { get; private set; } = string.Empty;
  public string JobName { get; private set; } = string.Empty;
  public SourceType SourceType { get; private set; } = SourceType.Unknown;
  public BackupStatus Status { get; private set; } = BackupStatus.Unknown;
  public DateTime StartedAtUtc { get; private set; }
  public DateTime? FinishedAtUtc { get; private set; }
  public int DurationSeconds { get; private set; }
  public long DataSizeBytes { get; private set; }
  public long BackupSizeBytes { get; private set; }
  public long TransferredSizeBytes { get; private set; }
  public string? Message { get; private set; }
  public DateTime ImportedAtUtc { get; private set; } = DateTime.UtcNow;

  private BackupExecution()
  {
  }

  public BackupExecution(
      string clientName,
      string jobName,
      SourceType sourceType,
      BackupStatus status,
      DateTime startedAtUtc,
      DateTime? finishedAtUtc,
      int durationSeconds,
      long dataSizeBytes,
      long backupSizeBytes,
      long transferredSizeBytes,
      string? message)
  {
    ClientName = NormalizeRequired(clientName, nameof(clientName), 150);
    JobName = NormalizeRequired(jobName, nameof(jobName), 200);
    SourceType = sourceType;
    Status = status;
    StartedAtUtc = EnsureUtc(startedAtUtc);
    FinishedAtUtc = finishedAtUtc.HasValue ? EnsureUtc(finishedAtUtc.Value) : null;
    DurationSeconds = durationSeconds < 0 ? 0 : durationSeconds;
    DataSizeBytes = dataSizeBytes < 0 ? 0 : dataSizeBytes;
    BackupSizeBytes = backupSizeBytes < 0 ? 0 : backupSizeBytes;
    TransferredSizeBytes = transferredSizeBytes < 0 ? 0 : transferredSizeBytes;
    Message = NormalizeOptional(message, 1000);
    ImportedAtUtc = DateTime.UtcNow;
  }

  public void UpdateStatus(BackupStatus status, string? message = null)
  {
    Status = status;
    Message = NormalizeOptional(message, 1000);
  }

  private static string NormalizeRequired(string value, string paramName, int maxLength)
  {
    if (string.IsNullOrWhiteSpace(value))
      throw new ArgumentException("Value cannot be empty.", paramName);

    value = value.Trim();

    if (value.Length > maxLength)
      throw new ArgumentException($"Value cannot exceed {maxLength} characters.", paramName);

    return value;
  }

  private static string? NormalizeOptional(string? value, int maxLength)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;

    value = value.Trim();

    if (value.Length > maxLength)
      return value[..maxLength];

    return value;
  }

  private static DateTime EnsureUtc(DateTime dateTime)
  {
    return dateTime.Kind switch
    {
      DateTimeKind.Utc => dateTime,
      DateTimeKind.Local => dateTime.ToUniversalTime(),
      _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
    };
  }
}
