using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BackupIngestion.Api.Contracts.Requests;

public sealed class ImportJsonFileRequest
{
  [Required]
  public IFormFile File { get; init; } = default!;
}
