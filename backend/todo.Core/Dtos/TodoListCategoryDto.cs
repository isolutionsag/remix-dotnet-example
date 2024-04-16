using System.Diagnostics.CodeAnalysis;

namespace todo.Core.Dtos;

[ExcludeFromCodeCoverage]
public record TodoListCategoryDto(Guid Id, string Name);