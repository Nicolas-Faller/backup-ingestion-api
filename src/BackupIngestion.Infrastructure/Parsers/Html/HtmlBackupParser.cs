using System.Globalization;
using System.Text;
using BackupIngestion.Application.Abstractions.Parsing;
using BackupIngestion.Domain.Entities;
using BackupIngestion.Domain.Enums;
using HtmlAgilityPack;

namespace BackupIngestion.Infrastructure.Parsers.Html;

public class HtmlBackupParser : IHtmlBackupParser
{
  public IReadOnlyList<BackupExecution> Parse(string htmlContent)
  {
    if (string.IsNullOrWhiteSpace(htmlContent))
      throw new ArgumentException("HTML content cannot be empty.", nameof(htmlContent));

    var document = new HtmlDocument();
    document.LoadHtml(htmlContent);

    var rows = ParseRows(document);

    var executions = new List<BackupExecution>();

    foreach (var row in rows)
    {
      var durationSeconds = row.DurationSeconds
                            ?? CalculateDuration(row.StartedAtUtc, row.FinishedAtUtc);

      var execution = new BackupExecution(
          clientName: row.ClientName,
          jobName: row.JobName,
          sourceType: SourceType.Html,
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

  private static IReadOnlyList<HtmlBackupRow> ParseRows(HtmlDocument document)
  {
    var table = document.DocumentNode.SelectSingleNode("//table[@id='backup-executions']");
    if (table is null)
      throw new InvalidOperationException("Could not find table with id 'backup-executions'.");

    var bodyRows = table.SelectNodes(".//tbody/tr") ?? table.SelectNodes(".//tr");
    if (bodyRows is null || bodyRows.Count == 0)
      return Array.Empty<HtmlBackupRow>();

    var result = new List<HtmlBackupRow>();

    foreach (var tr in bodyRows)
    {
      var cells = tr.SelectNodes("./td");
      if (cells is null || cells.Count < 10)
        continue;

      var row = new HtmlBackupRow
      {
        ClientName = GetCellText(cells, 0),
        JobName = GetCellText(cells, 1),
        Status = GetCellText(cells, 2),
        StartedAtUtc = ParseDateTime(GetCellText(cells, 3)),
        FinishedAtUtc = ParseNullableDateTime(GetCellText(cells, 4)),
        DurationSeconds = ParseNullableInt(GetCellText(cells, 5)),
        DataSizeBytes = ParseLong(GetCellText(cells, 6)),
        BackupSizeBytes = ParseLong(GetCellText(cells, 7)),
        TransferredSizeBytes = ParseLong(GetCellText(cells, 8)),
        Message = NormalizeOptional(GetCellText(cells, 9))
      };

      result.Add(row);
    }

    return result;
  }

  private static string GetCellText(HtmlNodeCollection cells, int index)
  {
    if (index < 0 || index >= cells.Count)
      return string.Empty;

    var raw = HtmlEntity.DeEntitize(cells[index].InnerText);
    return CollapseWhitespace(raw);
  }

  private static string CollapseWhitespace(string input)
  {
    if (string.IsNullOrWhiteSpace(input))
      return string.Empty;

    var builder = new StringBuilder();
    var previousWasWhitespace = false;

    foreach (var ch in input.Trim())
    {
      if (char.IsWhiteSpace(ch))
      {
        if (!previousWasWhitespace)
        {
          builder.Append(' ');
          previousWasWhitespace = true;
        }
      }
      else
      {
        builder.Append(ch);
        previousWasWhitespace = false;
      }
    }

    return builder.ToString();
  }

  private static string? NormalizeOptional(string value)
  {
    return string.IsNullOrWhiteSpace(value) ? null : value;
  }

  private static BackupStatus ParseStatus(string? status)
  {
    if (string.IsNullOrWhiteSpace(status))
      return BackupStatus.Unknown;

    return Enum.TryParse<BackupStatus>(status, ignoreCase: true, out var parsed)
        ? parsed
        : BackupStatus.Unknown;
  }

  private static DateTime ParseDateTime(string value)
  {
    if (DateTime.TryParse(
            value,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out var parsed))
    {
      return parsed;
    }

    throw new FormatException($"Invalid datetime value: '{value}'.");
  }

  private static DateTime? ParseNullableDateTime(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;

    if (DateTime.TryParse(
            value,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out var parsed))
    {
      return parsed;
    }

    return null;
  }

  private static int? ParseNullableInt(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;

    return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
        ? parsed
        : null;
  }

  private static long ParseLong(string value)
  {
    return long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
        ? parsed
        : 0;
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
