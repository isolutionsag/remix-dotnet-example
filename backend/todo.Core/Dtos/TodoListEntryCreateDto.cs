using System.Diagnostics.CodeAnalysis;

namespace todo.Core.Dtos;

[ExcludeFromCodeCoverage]
public record TodoListEntryCreateDto(string Title, string Description, Guid ParentId);