namespace todo.Core.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    TEntity GetById(Guid id);
    IEnumerable<TEntity> GetAll();
    Task<int> SaveChangesAsync(CancellationToken token = default);

}