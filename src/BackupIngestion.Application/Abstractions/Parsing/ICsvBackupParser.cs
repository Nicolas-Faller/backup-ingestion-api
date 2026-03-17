using BackupIngestion.Domain.Entities;

namespace BackupIngestion.Application.Abstractions.Parsing;

public interface ICsvBackupParser
{
  IReadOnlyList<BackupExecution> Parse(string csvContent);
}
