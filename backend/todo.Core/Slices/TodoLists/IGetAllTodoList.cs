using todo.Core.Dtos;

namespace todo.Core.Slices.TodoLists;

public interface IGetAllTodoList
{
    IEnumerable<TodoListDto> Execute(Guid? id = null);
}