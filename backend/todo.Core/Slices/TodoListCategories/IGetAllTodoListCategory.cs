using todo.Core.Dtos;

namespace todo.Core.Slices.TodoListCategories;

public interface IGetAllTodoListCategory
{
    IEnumerable<TodoListCategoryDto> Execute();
}