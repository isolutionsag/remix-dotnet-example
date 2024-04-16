using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo.Core.Dtos;
using todo.Core.Repositories;
using todo.Core.Slices.TodoListCategories;

namespace todo.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TodoListCategoryController : TodoControllerBase
{

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Create(
        [FromServices] ICreateTodoListCategory createTodoListCategory,
        [FromBody] TodoListCategoryCreateDto dto) =>
        await ExecuteDomainOperationAsync(createTodoListCategory, dto, result =>
        {
            var routeValues = new { id = result.Result.Id };
            return CreatedAtAction("Single", routeValues, result.Result.ToDto());
        });

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(
        [FromServices] IUpdateTodoListCategory updateTodoListCategory,
        [FromBody] TodoListCategoryDto dto) =>
        await ExecuteDomainOperationAsync(updateTodoListCategory, dto);

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(
        [FromServices] IRemoveTodoListCategory removeTodoListCategory,
        Guid id) =>
        await ExecuteDomainOperationAsync(removeTodoListCategory, id);

    [HttpGet]
    public IEnumerable<TodoListCategoryDto> All(
        [FromServices] IGetAllTodoListCategory getAllTodoListCategory) =>
        getAllTodoListCategory.Execute();

    [HttpGet("{id}")]
    public TodoListCategoryDto Single(
        Guid id,
        [FromServices] ITodoListCategoryRepository repo) =>
        repo.GetById(id).ToDto();
}