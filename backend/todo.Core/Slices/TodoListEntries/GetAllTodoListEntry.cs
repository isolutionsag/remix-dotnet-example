using todo.Core.Dtos;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoListEntries;

public class GetAllTodoListEntry(ITodoListEntryRepository repository) : IGetAllTodoListEntry
{
    public IEnumerable<TodoListEntryDto> Execute(Guid? id) =>
    (
        id.HasValue
            ? repository.GetAllByListId(id.Value)
            : repository.GetAll()
    ).Select(x => x.ToDto());
}