using Microsoft.EntityFrameworkCore;
using todo.Core.Entities;

namespace todo.Infrastructure;

public interface ITodoContext
{
    public DbSet<TodoListCategory> TodoListCategories { get; set; }
    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<TodoListEntry> TodoListEntries { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}