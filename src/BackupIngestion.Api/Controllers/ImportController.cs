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
  private readonly IHtmlBackupImportService _htmlBackupImportService;

  public ImportController(
      IJsonBackupImportService jsonBackupImportService,
      ICsvBackupImportService csvBackupImportService,
      IHtmlBackupImportService htmlBackupImportService)
  {
    _jsonBackupImportService = jsonBackupImportService;
    _csvBackupImportService = csvBackupImportService;
    _htmlBackupImportService = htmlBackupImportService;
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

  [HttpPost("html")]
  [Consumes("multipart/form-data")]
  [ProducesResponseType(typeof(ImportHtmlBackupsResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> ImportHtml(
      [FromForm] ImportHtmlFileRequest request,
      CancellationToken cancellationToken)
  {
    if (request.File is null || request.File.Length == 0)
      return BadRequest("An HTML file must be provided.");

    string htmlContent;

    using (var stream = request.File.OpenReadStream())
    using (var reader = new StreamReader(stream))
    {
      htmlContent = await reader.ReadToEndAsync();
    }

    var result = await _htmlBackupImportService.ImportAsync(htmlContent, cancellationToken);

    return Ok(new ImportHtmlBackupsResponse(result.ImportedCount));
  }
}
