using BackupIngestion.Application.Abstractions.Parsing;
using BackupIngestion.Application.Abstractions.Persistence;
using BackupIngestion.Application.Abstractions.Services;
using BackupIngestion.Application.DTOs;

namespace BackupIngestion.Application.Services;

public class CsvBackupImportService : ICsvBackupImportService
{
  private readonly ICsvBackupParser _parser;
  private readonly IBackupExecutionRepository _repository;

  public CsvBackupImportService(
      ICsvBackupParser parser,
      IBackupExecutionRepository repository)
  {
    _parser = parser;
    _repository = repository;
  }

  public async Task<ImportCsvResultDto> ImportAsync(
      string csvContent,
      CancellationToken cancellationToken = default)
  {
    var executions = _parser.Parse(csvContent);

    if (executions.Count == 0)
      return new ImportCsvResultDto(0);

    await _repository.AddRangeAsync(executions, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return new ImportCsvResultDto(executions.Count);
  }
}
