using Microsoft.EntityFrameworkCore;
using todo.Core.Entities;
using todo.Core.Exceptions;
using todo.Core.Repositories;

namespace todo.Infrastructure.Repositories;

public abstract class BaseRepository<T>(DbSet<T> table, ITodoContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly ITodoContext DbContext = context;

    protected abstract IQueryable<T> BaseIncludes(DbSet<T> table);

    public void Add(T entity) =>
        table.Add(entity);

    public void Update(T entity) =>
        table.Update(entity);

    public void Remove(T entity) =>
        table.Remove(entity);

    public T GetById(Guid id) =>
        BaseIncludes(table).FirstOrDefault(x => x.Id == id) ?? throw new EntityNotFoundException(id, typeof(T));

    public virtual IEnumerable<T> GetAll() =>
        BaseIncludes(table);

    public Task<int> SaveChangesAsync(CancellationToken token = default) =>
        context.SaveChangesAsync(token);
}