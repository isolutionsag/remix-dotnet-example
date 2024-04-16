using todo.Core.Dtos;
using todo.Core.Entities;

namespace todo.Core.Slices.TodoLists;

public interface ICreateTodoList : IDomainOperationAsync<TodoList, TodoListCreateDto>
{
}