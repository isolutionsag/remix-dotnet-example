using Microsoft.EntityFrameworkCore;
using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Infrastructure.Repositories;

public class TodoListRepository(ITodoContext dbContext)
    : BaseRepository<TodoList>(dbContext.TodoLists, dbContext), ITodoListRepository
{
    public IEnumerable<TodoList> GetAllByCategoryId(Guid id) =>
        BaseIncludes(DbContext.TodoLists)
            .Where(x => x.CategoryId == id)
            .ToList();

    protected override IQueryable<TodoList> BaseIncludes(DbSet<TodoList> table) =>
        table
            .Include(x => x.Category)
            .Include(x => x.Entries)
            .AsQueryable();
}