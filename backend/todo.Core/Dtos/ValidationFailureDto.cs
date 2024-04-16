using System.Diagnostics.CodeAnalysis;

namespace todo.Core.Dtos;

[ExcludeFromCodeCoverage]
public record ValidationFailureDto(string PropertyName, string Code, string Message);