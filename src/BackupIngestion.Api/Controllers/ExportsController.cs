using BackupIngestion.Api.Contracts.Requests;
using BackupIngestion.Application.Abstractions.Services;
using BackupIngestion.Application.DTOs;
using BackupIngestion.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BackupIngestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportsController : ControllerBase
{
  private readonly IBackupExcelExportService _backupExcelExportService;

  public ExportsController(IBackupExcelExportService backupExcelExportService)
  {
    _backupExcelExportService = backupExcelExportService;
  }

  [HttpGet("excel")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> ExportExcel(
      [FromQuery] GetBackupsRequest request,
      CancellationToken cancellationToken)
  {
    BackupStatus? status = null;
    if (!string.IsNullOrWhiteSpace(request.Status))
    {
      if (!Enum.TryParse<BackupStatus>(request.Status, true, out var parsedStatus))
        return BadRequest("Invalid status. Use: Success, Warning, Failed, Running or Unknown.");

      status = parsedStatus;
    }

    SourceType? sourceType = null;
    if (!string.IsNullOrWhiteSpace(request.SourceType))
    {
      if (!Enum.TryParse<SourceType>(request.SourceType, true, out var parsedSourceType))
        return BadRequest("Invalid sourceType. Use: Json, Csv or Html.");

      sourceType = parsedSourceType;
    }

    var searchParams = new BackupSearchParamsDto(
        request.ClientName,
        status,
        sourceType,
        request.StartDate,
        request.EndDate,
        1,
        1);

    var fileBytes = await _backupExcelExportService.ExportAsync(searchParams, cancellationToken);

    var fileName = $"backup-report-{DateTime.UtcNow:yyyyMMdd-HHmmss}.xlsx";

    return File(
        fileBytes,
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        fileName);
  }
}
