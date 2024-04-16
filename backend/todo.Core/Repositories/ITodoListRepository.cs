using todo.Core.Entities;

namespace todo.Core.Repositories;

public interface ITodoListRepository : IRepository<TodoList>
{
    IEnumerable<TodoList> GetAllByCategoryId(Guid id);
}