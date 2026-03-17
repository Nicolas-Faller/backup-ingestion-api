using BackupIngestion.Application.Abstractions.Persistence;
using BackupIngestion.Application.Abstractions.Services;
using BackupIngestion.Application.DTOs;
using ClosedXML.Excel;

namespace BackupIngestion.Infrastructure.Exports.Excel;

public class BackupExcelExportService : IBackupExcelExportService
{
  private readonly IBackupExecutionRepository _repository;

  public BackupExcelExportService(IBackupExecutionRepository repository)
  {
    _repository = repository;
  }

  public async Task<byte[]> ExportAsync(
      BackupSearchParamsDto searchParams,
      CancellationToken cancellationToken = default)
  {
    var backups = await _repository.ListAsync(searchParams, cancellationToken);

    using var workbook = new XLWorkbook();
    var worksheet = workbook.Worksheets.Add("Backups");

    var headers = new[]
    {
            "Id",
            "ClientName",
            "JobName",
            "SourceType",
            "Status",
            "StartedAtUtc",
            "FinishedAtUtc",
            "DurationSeconds",
            "DataSizeBytes",
            "BackupSizeBytes",
            "TransferredSizeBytes",
            "Message",
            "ImportedAtUtc"
        };

    for (var i = 0; i < headers.Length; i++)
    {
      worksheet.Cell(1, i + 1).Value = headers[i];
    }

    var headerRange = worksheet.Range(1, 1, 1, headers.Length);
    headerRange.Style.Font.Bold = true;
    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

    var row = 2;

    foreach (var backup in backups)
    {
      worksheet.Cell(row, 1).Value = backup.Id.ToString();
      worksheet.Cell(row, 2).Value = backup.ClientName;
      worksheet.Cell(row, 3).Value = backup.JobName;
      worksheet.Cell(row, 4).Value = backup.SourceType.ToString();
      worksheet.Cell(row, 5).Value = backup.Status.ToString();
      worksheet.Cell(row, 6).Value = backup.StartedAtUtc;
      worksheet.Cell(row, 7).Value = backup.FinishedAtUtc;
      worksheet.Cell(row, 8).Value = backup.DurationSeconds;
      worksheet.Cell(row, 9).Value = backup.DataSizeBytes;
      worksheet.Cell(row, 10).Value = backup.BackupSizeBytes;
      worksheet.Cell(row, 11).Value = backup.TransferredSizeBytes;
      worksheet.Cell(row, 12).Value = backup.Message;
      worksheet.Cell(row, 13).Value = backup.ImportedAtUtc;

      row++;
    }

    if (row > 2)
    {
      worksheet.Range(1, 1, row - 1, headers.Length).SetAutoFilter();
    }

    worksheet.SheetView.FreezeRows(1);
    worksheet.Columns().AdjustToContents();

    using var stream = new MemoryStream();
    workbook.SaveAs(stream);

    return stream.ToArray();
  }
}
