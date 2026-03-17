namespace BackupIngestion.Api.Contracts.Responses;

public sealed record BackupSummaryResponse(
    int TotalBackups,
    int TotalClients,
    long TotalTransferredBytes,
    double AverageDurationSeconds,
    IReadOnlyList<StatusCountResponse> StatusCounts,
    IReadOnlyList<SourceTypeCountResponse> SourceTypeCounts);

public sealed record StatusCountResponse(
    string Status,
    int Count);

public sealed record SourceTypeCountResponse(
    string SourceType,
    int Count);
