using System.Diagnostics.CodeAnalysis;

namespace todo.Core.Dtos;

[ExcludeFromCodeCoverage]
public record TodoListEntryDto(Guid Id, string Title, string Description, bool IsCompleted, Guid ParentId, TodoListDto? Parent);