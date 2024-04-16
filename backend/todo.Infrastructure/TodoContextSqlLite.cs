using Microsoft.EntityFrameworkCore;
using todo.Core.Entities;

namespace todo.Infrastructure;

public class TodoContextSqlLite : DbContext, ITodoContext
{
    public string DbPath { get; }

    public TodoContextSqlLite()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "todo.db");
    }

    public DbSet<TodoListCategory> TodoListCategories { get; set; }
    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<TodoListEntry> TodoListEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
        base.OnConfiguring(optionsBuilder);
    }
}