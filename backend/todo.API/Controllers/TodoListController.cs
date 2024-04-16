using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo.Core.Dtos;
using todo.Core.Repositories;
using todo.Core.Slices.TodoLists;

namespace todo.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TodoListController : TodoControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] ICreateTodoList createTodoList,
        [FromBody] TodoListCreateDto dto) =>
        await ExecuteDomainOperationAsync(createTodoList, dto, result =>
        {
            var routeValues = new { id = result.Result.Id };
            return CreatedAtAction("Single", routeValues, result.Result.ToDto());
        });

    [HttpPut]
    public async Task<ActionResult> Update(
        [FromServices] IUpdateTodoList updateTodoList,
        [FromBody] TodoListDto dto) =>
        await ExecuteDomainOperationAsync(updateTodoList, dto);

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(
        [FromServices] IRemoveTodoList removeTodoList,
         Guid id) =>
        await ExecuteDomainOperationAsync(removeTodoList, id);

    [HttpGet]
    public IEnumerable<TodoListDto> All(
        [FromServices] IGetAllTodoList getAllTodoList) =>
        getAllTodoList.Execute();

    [HttpGet("category/{id}")]
    public IEnumerable<TodoListDto> AllByListId(
        [FromServices] IGetAllTodoList getAllTodoList, Guid id) =>
        getAllTodoList.Execute(id);

    [HttpGet("{id}")]
    public TodoListDto Single(
        Guid id,
        [FromServices] ITodoListRepository repo) =>
        repo.GetById(id).ToDto();
}