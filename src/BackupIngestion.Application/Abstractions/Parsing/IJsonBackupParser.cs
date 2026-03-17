using BackupIngestion.Domain.Entities;

namespace BackupIngestion.Application.Abstractions.Parsing;

public interface IJsonBackupParser
{
  IReadOnlyList<BackupExecution> Parse(string jsonContent);
}
