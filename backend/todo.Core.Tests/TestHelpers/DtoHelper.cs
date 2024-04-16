using todo.Core.Dtos;

namespace todo.Core.Tests.TestHelpers;

public static class DtoHelper
{
    public static TodoListEntryCreateDto ValidTodoListEntryCreateDto() =>
        new("My title", "My description", Guid.NewGuid());

    public static TodoListEntryCreateDto InvalidTodoListEntryCreateDto() =>
        new(string.Empty, "My description", Guid.Empty);

    public static TodoListEntryDto ValidTodoListEntryDto() =>
        new(Guid.NewGuid(), "My title", "My description", false, Guid.NewGuid(), ValidTodoListDto());

    public static TodoListEntryDto InvalidTodoListEntryDto() =>
        new(Guid.Empty, string.Empty, "My description", false, Guid.Empty, null);

    public static TodoListCategoryCreateDto ValidTodoListCategoryCreateDto() =>
        new("My Name");

    public static TodoListCategoryCreateDto InvalidTodoListCategoryCreateDto() =>
        new(string.Empty);

    public static TodoListCategoryDto ValidTodoListCategoryDto() =>
        new(Guid.NewGuid(), "My Name");

    public static TodoListCategoryDto InvalidTodoListCategoryDto() =>
        new(Guid.Empty, string.Empty);

    public static TodoListCreateDto ValidTodoListCreateDto() =>
        new("My Name", Guid.NewGuid());

    public static TodoListCreateDto ValidEmptyTodoListCreateDto() =>
        new(string.Empty, Guid.Empty);

    public static TodoListDto ValidTodoListDto() =>
        new(Guid.NewGuid(), "My Name", Guid.NewGuid(), ValidTodoListCategoryDto());

    public static TodoListDto InvalidTodoListDto() =>
        new(Guid.Empty, string.Empty, Guid.Empty, null);
}