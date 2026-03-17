using System.Globalization;
using BackupIngestion.Application.Abstractions.Parsing;
using BackupIngestion.Domain.Entities;
using BackupIngestion.Domain.Enums;
using CsvHelper;
using CsvHelper.Configuration;

namespace BackupIngestion.Infrastructure.Parsers.Csv;

public class CsvBackupParser : ICsvBackupParser
{
  public IReadOnlyList<BackupExecution> Parse(string csvContent)
  {
    if (string.IsNullOrWhiteSpace(csvContent))
      throw new ArgumentException("CSV content cannot be empty.", nameof(csvContent));

    using var reader = new StringReader(csvContent);

    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      PrepareHeaderForMatch = args => args.Header.Trim(),
      MissingFieldFound = null,
      HeaderValidated = null
    };

    using var csv = new CsvReader(reader, config);

    var rows = csv.GetRecords<CsvBackupRow>().ToList();

    var executions = new List<BackupExecution>();

    foreach (var row in rows)
    {
      var durationSeconds = row.DurationSeconds
                            ?? CalculateDuration(row.StartedAtUtc, row.FinishedAtUtc);

      var execution = new BackupExecution(
          clientName: row.ClientName ?? string.Empty,
          jobName: row.JobName ?? string.Empty,
          sourceType: SourceType.Csv,
          status: ParseStatus(row.Status),
          startedAtUtc: row.StartedAtUtc,
          finishedAtUtc: row.FinishedAtUtc,
          durationSeconds: durationSeconds,
          dataSizeBytes: row.DataSizeBytes,
          backupSizeBytes: row.BackupSizeBytes,
          transferredSizeBytes: row.TransferredSizeBytes,
          message: row.Message);

      executions.Add(execution);
    }

    return executions;
  }

  private static BackupStatus ParseStatus(string? status)
  {
    if (string.IsNullOrWhiteSpace(status))
      return BackupStatus.Unknown;

    return Enum.TryParse<BackupStatus>(status, ignoreCase: true, out var parsed)
        ? parsed
        : BackupStatus.Unknown;
  }

  private static int CalculateDuration(DateTime startedAtUtc, DateTime? finishedAtUtc)
  {
    if (!finishedAtUtc.HasValue)
      return 0;

    var duration = finishedAtUtc.Value - startedAtUtc;

    if (duration.TotalSeconds < 0)
      return 0;

    return (int)duration.TotalSeconds;
  }
}
