using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo.Core.Dtos;
using todo.Core.Repositories;
using todo.Core.Slices.TodoListEntries;

namespace todo.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TodoListEntryController : TodoControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] ICreateTodoListEntry createTodoList,
        [FromBody] TodoListEntryCreateDto dto) =>
        await ExecuteDomainOperationAsync(createTodoList, dto, result =>
        {
            var routeValues = new { id = result.Result.Id };
            return CreatedAtAction("Single", routeValues, result.Result.ToDto());
        });

    [HttpPut]
    public async Task<ActionResult> Update(
        [FromServices] IUpdateTodoListEntry updateTodoList,
        [FromBody] TodoListEntryDto dto) =>
        await ExecuteDomainOperationAsync(updateTodoList, dto);

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(
        [FromServices] IRemoveTodoListEntry removeTodoList,
        Guid id) =>
        await ExecuteDomainOperationAsync(removeTodoList, id);

    [HttpGet]
    public IEnumerable<TodoListEntryDto> All(
        [FromServices] IGetAllTodoListEntry getAllTodoListEntry) =>
        getAllTodoListEntry.Execute();

    [HttpGet("list/{id}")]
    public IEnumerable<TodoListEntryDto> AllByParent(
        [FromServices] IGetAllTodoListEntry getAllTodoListEntry, Guid id) =>
        getAllTodoListEntry.Execute(id);

    [HttpGet("{id}")]
    public TodoListEntryDto Single(
        Guid id,
        [FromServices] ITodoListEntryRepository repo) =>
        repo.GetById(id).ToDto();
}

