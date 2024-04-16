using FluentValidation;
using todo.Core.Dtos;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


namespace todo.Core.Entities;

public class TodoListCategory : BaseEntity
{
    public TodoListCategory()
    {
    }

    private TodoListCategory(TodoListCategoryCreateDto dto)
    {
        Id = Guid.NewGuid();
        Name = dto.Name;
    }

    private TodoListCategory(TodoListCategoryDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
    }

    public string Name { get; set; }

    public virtual ICollection<TodoList> Lists { get; } = new List<TodoList>();

    public TodoListCategoryDto ToDto() => new(Id, Name);

    public static DomainOperationResult<TodoListCategory> Create(TodoListCategoryCreateDto dto)
    {
        var entity = new TodoListCategory(dto);
        var validator = new TodoListCategoryValidator();
        var validation = validator.Validate(entity);

        return new DomainOperationResult<TodoListCategory>(entity, validation.Errors);
    }

    public static DomainOperationResult<TodoListCategory> UpdateOrDelete(TodoListCategoryDto dto)
    {
        var entity = new TodoListCategory(dto);
        var validator = new TodoListCategoryValidator();
        var validation = validator.Validate(entity);

        return new DomainOperationResult<TodoListCategory>(entity, validation.Errors);
    }

    private class TodoListCategoryValidator : AbstractValidator<TodoListCategory>
    {
        public TodoListCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}