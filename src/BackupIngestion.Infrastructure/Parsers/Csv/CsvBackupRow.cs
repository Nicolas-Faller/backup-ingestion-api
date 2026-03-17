namespace BackupIngestion.Infrastructure.Parsers.Csv;

internal sealed class CsvBackupRow
{
  public string? ClientName { get; set; }
  public string? JobName { get; set; }
  public string? Status { get; set; }
  public DateTime StartedAtUtc { get; set; }
  public DateTime? FinishedAtUtc { get; set; }
  public int? DurationSeconds { get; set; }
  public long DataSizeBytes { get; set; }
  public long BackupSizeBytes { get; set; }
  public long TransferredSizeBytes { get; set; }
  public string? Message { get; set; }
}
