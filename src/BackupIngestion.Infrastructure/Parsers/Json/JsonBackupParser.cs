using System.Text.Json;
using BackupIngestion.Application.Abstractions.Parsing;
using BackupIngestion.Domain.Entities;
using BackupIngestion.Domain.Enums;

namespace BackupIngestion.Infrastructure.Parsers.Json;

public class JsonBackupParser : IJsonBackupParser
{
  public IReadOnlyList<BackupExecution> Parse(string jsonContent)
  {
    if (string.IsNullOrWhiteSpace(jsonContent))
      throw new ArgumentException("JSON content cannot be empty.", nameof(jsonContent));

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var payloads = JsonSerializer.Deserialize<List<JsonBackupPayload>>(jsonContent, options);

    if (payloads is null)
      throw new InvalidOperationException("Could not deserialize JSON content.");

    var executions = new List<BackupExecution>();

    foreach (var payload in payloads)
    {
      var durationSeconds = payload.DurationSeconds
                            ?? CalculateDuration(payload.StartedAtUtc, payload.FinishedAtUtc);

      var execution = new BackupExecution(
          clientName: payload.ClientName ?? string.Empty,
          jobName: payload.JobName ?? string.Empty,
          sourceType: SourceType.Json,
          status: ParseStatus(payload.Status),
          startedAtUtc: payload.StartedAtUtc,
          finishedAtUtc: payload.FinishedAtUtc,
          durationSeconds: durationSeconds,
          dataSizeBytes: payload.DataSizeBytes,
          backupSizeBytes: payload.BackupSizeBytes,
          transferredSizeBytes: payload.TransferredSizeBytes,
          message: payload.Message);

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
