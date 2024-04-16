using Microsoft.AspNetCore.Mvc;
using todo.Core;
using todo.Core.Slices;

namespace todo.API.Controllers;

public abstract class TodoControllerBase : ControllerBase
{
    protected async Task<ActionResult> ExecuteDomainOperationAsync<TEntity, TDto>(
        IDomainOperationAsync<TEntity, TDto> domainOperationAsync,
        TDto dto,
        Func<DomainOperationResult<TEntity>, ActionResult>? resultFunc = null)
    {
        var result = await domainOperationAsync.ExecuteAsync(dto);
        if (result.IsSuccess())
        {
            return resultFunc is null ? Ok(result) : resultFunc(result);
        }
        return result.ProduceErrorResponse();
    }
}