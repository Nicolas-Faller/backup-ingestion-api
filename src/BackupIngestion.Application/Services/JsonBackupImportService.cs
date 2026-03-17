using BackupIngestion.Application.Abstractions.Parsing;
using BackupIngestion.Application.Abstractions.Persistence;
using BackupIngestion.Application.Abstractions.Services;
using BackupIngestion.Application.DTOs;

namespace BackupIngestion.Application.Services;

public class JsonBackupImportService : IJsonBackupImportService
{
  private readonly IJsonBackupParser _parser;
  private readonly IBackupExecutionRepository _repository;

  public JsonBackupImportService(
      IJsonBackupParser parser,
      IBackupExecutionRepository repository)
  {
    _parser = parser;
    _repository = repository;
  }

  public async Task<ImportJsonResultDto> ImportAsync(
      string jsonContent,
      CancellationToken cancellationToken = default)
  {
    var executions = _parser.Parse(jsonContent);

    if (executions.Count == 0)
      return new ImportJsonResultDto(0);

    await _repository.AddRangeAsync(executions, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return new ImportJsonResultDto(executions.Count);
  }
}
