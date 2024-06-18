using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Api.Endpoints.Results;

public class ValidationProblemResult(IEnumerable<string> errors) : ProblemDetails
{
    [JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; set; } = errors;
}
