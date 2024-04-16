using Microsoft.EntityFrameworkCore;
using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Infrastructure.Repositories;

public class TodoListEntryRepository(ITodoContext dbContext)
    : BaseRepository<TodoListEntry>(dbContext.TodoListEntries, dbContext), ITodoListEntryRepository
{
    public IEnumerable<TodoListEntry> GetAllByListId(Guid id) =>
        BaseIncludes(DbContext.TodoListEntries)
            .Where(x => x.ParentId == id)
            .ToList();

    protected override IQueryable<TodoListEntry> BaseIncludes(DbSet<TodoListEntry> table) =>
        table
            .Include(x => x.Parent)
            .AsQueryable();
}