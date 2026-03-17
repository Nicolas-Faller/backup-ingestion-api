namespace BackupIngestion.Api.Contracts.Responses;

public sealed record BackupExecutionResponse(
    Guid Id,
    string ClientName,
    string JobName,
    string SourceType,
    string Status,
    DateTime StartedAtUtc,
    DateTime? FinishedAtUtc,
    int DurationSeconds,
    long DataSizeBytes,
    long BackupSizeBytes,
    long TransferredSizeBytes,
    string? Message,
    DateTime ImportedAtUtc);
