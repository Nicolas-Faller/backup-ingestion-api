namespace BackupIngestion.Application.DTOs;

public sealed record StatusCountDto(
    string Status,
    int Count);
