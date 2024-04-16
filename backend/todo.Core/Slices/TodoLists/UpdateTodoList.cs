using todo.Core.Dtos;
using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoLists;

public class UpdateTodoList(ITodoListRepository repository) : IUpdateTodoList
{
    public async Task<DomainOperationResult<TodoList>> ExecuteAsync(TodoListDto dto)
    {
        var todoList = TodoList.UpdateOrDelete(dto);

        if (todoList.IsSuccess())
        {
            repository.Update(todoList.Result);
            await repository.SaveChangesAsync();
        }

        return todoList;
    }
}