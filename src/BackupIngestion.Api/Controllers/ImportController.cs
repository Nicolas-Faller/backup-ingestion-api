using BackupIngestion.Api.Contracts.Requests;
using BackupIngestion.Api.Contracts.Responses;
using BackupIngestion.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackupIngestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
  private readonly IJsonBackupImportService _jsonBackupImportService;
  private readonly ICsvBackupImportService _csvBackupImportService;

  public ImportController(
      IJsonBackupImportService jsonBackupImportService,
      ICsvBackupImportService csvBackupImportService)
  {
    _jsonBackupImportService = jsonBackupImportService;
    _csvBackupImportService = csvBackupImportService;
  }

  [HttpPost("json")]
  [Consumes("multipart/form-data")]
  [ProducesResponseType(typeof(ImportJsonBackupsResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> ImportJson(
      [FromForm] ImportJsonFileRequest request,
      CancellationToken cancellationToken)
  {
    if (request.File is null || request.File.Length == 0)
      return BadRequest("A JSON file must be provided.");

    string jsonContent;

    using (var stream = request.File.OpenReadStream())
    using (var reader = new StreamReader(stream))
    {
      jsonContent = await reader.ReadToEndAsync();
    }

    var result = await _jsonBackupImportService.ImportAsync(jsonContent, cancellationToken);

    return Ok(new ImportJsonBackupsResponse(result.ImportedCount));
  }

  [HttpPost("csv")]
  [Consumes("multipart/form-data")]
  [ProducesResponseType(typeof(ImportCsvBackupsResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> ImportCsv(
      [FromForm] ImportCsvFileRequest request,
      CancellationToken cancellationToken)
  {
    if (request.File is null || request.File.Length == 0)
      return BadRequest("A CSV file must be provided.");

    string csvContent;

    using (var stream = request.File.OpenReadStream())
    using (var reader = new StreamReader(stream))
    {
      csvContent = await reader.ReadToEndAsync();
    }

    var result = await _csvBackupImportService.ImportAsync(csvContent, cancellationToken);

    return Ok(new ImportCsvBackupsResponse(result.ImportedCount));
  }
}
