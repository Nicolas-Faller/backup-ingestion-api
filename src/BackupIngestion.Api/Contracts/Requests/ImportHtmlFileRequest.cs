using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BackupIngestion.Api.Contracts.Requests;

public sealed class ImportHtmlFileRequest
{
  [Required]
  public IFormFile File { get; init; } = default!;
}
