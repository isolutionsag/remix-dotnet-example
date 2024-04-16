namespace todo.Core.Slices;

public interface IDomainOperation<TEntity, in TDto>
{
    Task<DomainOperationResult<TEntity>> Execute(TDto dto);
}