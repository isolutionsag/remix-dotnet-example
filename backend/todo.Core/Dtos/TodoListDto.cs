using System.Diagnostics.CodeAnalysis;

namespace todo.Core.Dtos;

[ExcludeFromCodeCoverage]
public record TodoListDto(Guid Id, string Name, Guid CategoryId, TodoListCategoryDto? Category);