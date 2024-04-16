using todo.Core.Dtos;
using todo.Core.Entities;

namespace todo.Core.Slices.TodoListCategories;

public interface ICreateTodoListCategory : IDomainOperationAsync<TodoListCategory, TodoListCategoryCreateDto>
{
}