using todo.Core.Dtos;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoLists;

public class GetAllTodoList(ITodoListRepository repository) : IGetAllTodoList
{
    public IEnumerable<TodoListDto> Execute(Guid? id) =>
        (
            id.HasValue
            ? repository.GetAllByCategoryId(id.Value)
            : repository.GetAll()
        ).Select(x => x.ToDto());
}