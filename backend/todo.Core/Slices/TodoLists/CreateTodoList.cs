using todo.Core.Dtos;
using todo.Core.Entities;
using todo.Core.Repositories;

namespace todo.Core.Slices.TodoLists;

public class CreateTodoList(ITodoListRepository repository) : ICreateTodoList
{
    public async Task<DomainOperationResult<TodoList>> ExecuteAsync(TodoListCreateDto dto)
    {
        var todoList = TodoList.Create(dto);

        if (todoList.IsSuccess())
        {
            repository.Add(todoList.Result);
            await repository.SaveChangesAsync();
        }

        return todoList;
    }
}