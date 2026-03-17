using BackupIngestion.Api.Contracts.Requests;
using BackupIngestion.Api.Contracts.Responses;
using BackupIngestion.Application.Abstractions.Persistence;
using BackupIngestion.Application.DTOs;
using BackupIngestion.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BackupIngestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BackupsController : ControllerBase
{
  private readonly IBackupExecutionRepository _repository;

  public BackupsController(IBackupExecutionRepository repository)
  {
    _repository = repository;
  }

  [HttpGet]
  [ProducesResponseType(typeof(PagedResponse<BackupExecutionResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> GetAll(
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
        request.Page,
        request.PageSize);

    var result = await _repository.SearchAsync(searchParams, cancellationToken);

    var items = result.Items
        .Select(BackupExecutionDto.FromEntity)
        .Select(x => new BackupExecutionResponse(
            x.Id,
            x.ClientName,
            x.JobName,
            x.SourceType,
            x.Status,
            x.StartedAtUtc,
            x.FinishedAtUtc,
            x.DurationSeconds,
            x.DataSizeBytes,
            x.BackupSizeBytes,
            x.TransferredSizeBytes,
            x.Message,
            x.ImportedAtUtc))
        .ToList();

    var response = new PagedResponse<BackupExecutionResponse>(
        items,
        result.TotalCount,
        result.Page,
        result.PageSize,
        result.TotalPages);

    return Ok(response);
  }

  [HttpGet("{id:guid}")]
  [ProducesResponseType(typeof(BackupExecutionResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
  {
    var item = await _repository.GetByIdAsync(id, cancellationToken);

    if (item is null)
      return NotFound();

    var dto = BackupExecutionDto.FromEntity(item);

    var response = new BackupExecutionResponse(
        dto.Id,
        dto.ClientName,
        dto.JobName,
        dto.SourceType,
        dto.Status,
        dto.StartedAtUtc,
        dto.FinishedAtUtc,
        dto.DurationSeconds,
        dto.DataSizeBytes,
        dto.BackupSizeBytes,
        dto.TransferredSizeBytes,
        dto.Message,
        dto.ImportedAtUtc);

    return Ok(response);
  }
}
