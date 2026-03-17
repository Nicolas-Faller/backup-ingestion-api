using BackupIngestion.Application.Abstractions.Parsing;
using BackupIngestion.Application.Abstractions.Persistence;
using BackupIngestion.Application.Abstractions.Services;
using BackupIngestion.Application.DTOs;

namespace BackupIngestion.Application.Services;

public class HtmlBackupImportService : IHtmlBackupImportService
{
  private readonly IHtmlBackupParser _parser;
  private readonly IBackupExecutionRepository _repository;

  public HtmlBackupImportService(
      IHtmlBackupParser parser,
      IBackupExecutionRepository repository)
  {
    _parser = parser;
    _repository = repository;
  }

  public async Task<ImportHtmlResultDto> ImportAsync(
      string htmlContent,
      CancellationToken cancellationToken = default)
  {
    var executions = _parser.Parse(htmlContent);

    if (executions.Count == 0)
      return new ImportHtmlResultDto(0);

    await _repository.AddRangeAsync(executions, cancellationToken);
    await _repository.SaveChangesAsync(cancellationToken);

    return new ImportHtmlResultDto(executions.Count);
  }
}
