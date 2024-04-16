using FluentValidation;
using todo.Core.Dtos;

namespace todo.Core.Entities;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class TodoListEntry : BaseEntity
{
    private TodoListEntry(TodoListEntryCreateDto dto)
    {
        Id = Guid.NewGuid();
        Description = dto.Description;
        ParentId = dto.ParentId;
        Title = dto.Title;
        IsCompleted = false;
    }

    private TodoListEntry(TodoListEntryDto dto)
    {
        Id = dto.Id;
        Description = dto.Description;
        ParentId = dto.ParentId;
        Title = dto.Title;
        IsCompleted = dto.IsCompleted;
    }
    public TodoListEntry()
    {
    }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsCompleted { get; set; }

    public virtual TodoList Parent { get; set; }
    public Guid ParentId { get; set; }

    public TodoListEntryDto ToDto() =>
        new(
            Id,
            Title,
            Description,
            IsCompleted,
            ParentId,
            Parent?.ToDto());

    public static DomainOperationResult<TodoListEntry> Create(TodoListEntryCreateDto dto)
    {
        var entity = new TodoListEntry(dto);
        var validator = new TodoListEntryValidator();
        var validation = validator.Validate(entity);

        return new DomainOperationResult<TodoListEntry>(entity, validation.Errors);
    }

    public static DomainOperationResult<TodoListEntry> UpdateOrDelete(TodoListEntryDto dto)
    {
        var entity = new TodoListEntry(dto);
        var validator = new TodoListEntryValidator();
        var validation = validator.Validate(entity);

        return new DomainOperationResult<TodoListEntry>(entity, validation.Errors);
    }

    private class TodoListEntryValidator : AbstractValidator<TodoListEntry>
    {
        public TodoListEntryValidator()
        {
            RuleFor(x => x.ParentId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
        }
    }
}