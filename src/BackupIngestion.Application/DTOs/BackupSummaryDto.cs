namespace BackupIngestion.Application.DTOs;

public sealed record BackupSummaryDto(
    int TotalBackups,
    int TotalClients,
    long TotalTransferredBytes,
    double AverageDurationSeconds,
    IReadOnlyList<StatusCountDto> StatusCounts,
    IReadOnlyList<SourceTypeCountDto> SourceTypeCounts);
