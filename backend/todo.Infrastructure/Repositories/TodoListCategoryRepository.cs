using Microsoft.EntityFrameworkCore;
using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Infrastructure.Repositories;
public class TodoListCategoryRepository(ITodoContext dbContext)
        : BaseRepository<TodoListCategory>(dbContext.TodoListCategories, dbContext), ITodoListCategoryRepository
{
    protected override IQueryable<TodoListCategory> BaseIncludes(DbSet<TodoListCategory> table) =>
        table
            .Include(x => x.Lists)
            .AsQueryable();
}

