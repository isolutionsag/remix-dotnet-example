using FluentValidation;
using todo.Core.Dtos;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace todo.Core.Entities;

public class TodoList : BaseEntity
{
    public TodoList()
    {
    }

    private TodoList(TodoListCreateDto dto)
    {
        Id = Guid.NewGuid();
        Name = dto.Name;
        CategoryId = dto.CategoryId;
    }

    private TodoList(TodoListDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        CategoryId = dto.CategoryId;
    }


    public string Name { get; set; }

    public virtual ICollection<TodoListEntry> Entries { get; } = new List<TodoListEntry>();

    public virtual TodoListCategory Category { get; set; }
    public Guid CategoryId { get; set; }


    public static DomainOperationResult<TodoList> Create(TodoListCreateDto dto)
    {
        var entity = new TodoList(dto);
        var validator = new TodoListValidator(isCreate: true);
        var validation = validator.Validate(entity);

        return new DomainOperationResult<TodoList>(entity, validation.Errors);
    }

    public static DomainOperationResult<TodoList> UpdateOrDelete(TodoListDto dto)
    {
        var entity = new TodoList(dto);
        var validator = new TodoListValidator();
        var validation = validator.Validate(entity);

        return new DomainOperationResult<TodoList>(entity, validation.Errors);
    }

    private class TodoListValidator : AbstractValidator<TodoList>
    {
        public TodoListValidator(bool isCreate = false)
        {
            if (!isCreate)
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.CategoryId).NotEmpty();
            }
        }
    }

    public TodoListDto ToDto() => new(this.Id, this.Name, this.CategoryId, this.Category?.ToDto());
}
