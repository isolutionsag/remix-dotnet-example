namespace todo.Core.Slices;

public interface IDomainOperationAsync<TEntity, in TDto>
{
    Task<DomainOperationResult<TEntity>> ExecuteAsync(TDto dto);
}