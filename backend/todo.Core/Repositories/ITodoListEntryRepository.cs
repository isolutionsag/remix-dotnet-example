using todo.Core.Entities;

namespace todo.Core.Repositories;

public interface ITodoListEntryRepository : IRepository<TodoListEntry>
{
    IEnumerable<TodoListEntry> GetAllByListId(Guid id);
}