using BackupIngestion.Domain.Entities;

namespace BackupIngestion.Application.Abstractions.Parsing;

public interface IHtmlBackupParser
{
  IReadOnlyList<BackupExecution> Parse(string htmlContent);
}
