using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BackupIngestion.Api.Contracts.Requests;

public sealed class ImportCsvFileRequest
{
  [Required]
  public IFormFile File { get; init; } = default!;
}
