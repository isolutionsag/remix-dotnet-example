using System.Diagnostics.CodeAnalysis;
namespace todo.Core.Dtos;

[ExcludeFromCodeCoverage]
public record TodoListCategoryCreateDto(string Name);