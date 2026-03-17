namespace BackupIngestion.Infrastructure.Parsers.Html;

internal sealed class HtmlBackupRow
{
  public string ClientName { get; init; } = string.Empty;
  public string JobName { get; init; } = string.Empty;
  public string Status { get; init; } = string.Empty;
  public DateTime StartedAtUtc { get; init; }
  public DateTime? FinishedAtUtc { get; init; }
  public int? DurationSeconds { get; init; }
  public long DataSizeBytes { get; init; }
  public long BackupSizeBytes { get; init; }
  public long TransferredSizeBytes { get; init; }
  public string? Message { get; init; }
}
