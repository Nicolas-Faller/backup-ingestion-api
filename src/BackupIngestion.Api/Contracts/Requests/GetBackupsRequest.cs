using System.ComponentModel.DataAnnotations;

namespace BackupIngestion.Api.Contracts.Requests;

public sealed class GetBackupsRequest
{
  public string? ClientName { get; init; }
  public string? Status { get; init; }
  public string? SourceType { get; init; }
  public DateTime? StartDate { get; init; }
  public DateTime? EndDate { get; init; }

  [Range(1, int.MaxValue)]
  public int Page { get; init; } = 1;

  [Range(1, 100)]
  public int PageSize { get; init; } = 20;
}
