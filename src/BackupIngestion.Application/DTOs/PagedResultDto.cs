namespace BackupIngestion.Application.DTOs;

public sealed record PagedResultDto<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize)
{
  public int TotalPages => TotalCount == 0
      ? 0
      : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
