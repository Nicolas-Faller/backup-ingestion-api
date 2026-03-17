using BackupIngestion.Api.Contracts.Responses;
using BackupIngestion.Application.Abstractions.Persistence;
using BackupIngestion.Application.DTOs;
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
  [ProducesResponseType(typeof(IEnumerable<BackupExecutionResponse>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
  {
    var items = await _repository.GetAllAsync(cancellationToken);

    var response = items
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
