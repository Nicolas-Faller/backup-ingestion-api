namespace BackupIngestion.Application.DTOs;

public sealed record SourceTypeCountDto(
    string SourceType,
    int Count);
