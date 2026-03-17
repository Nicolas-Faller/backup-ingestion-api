using BackupIngestion.Domain.Enums;

namespace BackupIngestion.Application.DTOs;

public sealed record BackupSearchParamsDto(
    string? ClientName,
    BackupStatus? Status,
    SourceType? SourceType,
    DateTime? StartDate,
    DateTime? EndDate,
    int Page,
    int PageSize);
