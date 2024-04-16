using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using todo.Core.Entities;
using todo.Core.Exceptions;
using todo.Infrastructure.Repositories;

namespace todo.Infrastructure.Tests.Repositories;

public class BaseRepositoryTests
{
    private readonly ITodoContext _dbContext;
    private readonly DbSet<TodoListCategory> _categories;
    private readonly MyRepo _repository;
    private readonly List<TodoListCategory> _categoryData;

    public BaseRepositoryTests()
    {
        _dbContext = A.Fake<ITodoContext>();
        _categories = A.Fake<DbSet<TodoListCategory>>();
        _categoryData = new List<TodoListCategory>();
        A.CallTo(() => _dbContext.TodoListCategories).Returns(_categories);

        _repository = new MyRepo(_dbContext);
    }

    [Fact]
    public void Add_Should_Add_Entity_To_Context()
    {
        // Arrange
        var category = new TodoListCategory { Id = Guid.NewGuid(), Name = "Test Category" };
        _categoryData.Add(category);
        _repository.Init(_categoryData);

        // Act
        _repository.Add(category);

        // Assert
        A.CallTo(() => _categories.Add(category)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Update_Should_Update_Entity_In_Context()
    {
        // Arrange
        var category = new TodoListCategory { Id = Guid.NewGuid(), Name = "Test Category" };
        _categoryData.Add(category);
        _repository.Init(_categoryData);

        // Act
        _repository.Update(category);

        // Assert
        A.CallTo(() => _categories.Update(category)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Remove_Should_Remove_Entity_From_Context()
    {
        // Arrange
        var category = new TodoListCategory { Id = Guid.NewGuid(), Name = "Test Category" };
        _categoryData.Add(category);
        _repository.Init(_categoryData);

        // Act
        _repository.Remove(category);

        // Assert
        A.CallTo(() => _categories.Remove(category)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void GetById_Should_Return_Entity_When_Found()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = new TodoListCategory { Id = categoryId, Name = "Test Category" };
        _categoryData.Add(category);
        _repository.Init(_categoryData);

        // Act
        var result = _repository.GetById(categoryId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(categoryId);
    }

    [Fact]
    public void GetById_Should_Throw_EntityNotFoundException_When_Not_Found()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _repository.Init(_categoryData);
        // Act
        Action act = () => _repository.GetById(categoryId);

        // Assert
        act.Should().Throw<EntityNotFoundException>()
            .WithMessage($"Could not find entity of type={typeof(TodoListCategory)} and id={categoryId}.");
    }

    [Fact]
    public void GetAll_Should_Return_All_Entities()
    {
        // Arrange
        var categories = new List<TodoListCategory>
            {
                new() { Id = Guid.NewGuid(), Name = "Category 1" },
                new() { Id = Guid.NewGuid(), Name = "Category 2" }
            };

        _repository.Init(categories);

        // Act
        var result = _repository.GetAll();

        // Assert
        result.Should().BeEquivalentTo(categories);
    }

    [Fact]
    public async Task SaveChangesAsync_Should_Invoke_SaveChangesAsync_On_Context()
    {
        // Act
        await _repository.SaveChangesAsync();

        // Assert
        A.CallTo(() => _dbContext.SaveChangesAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
    }
    private class MyRepo(ITodoContext dbContext)
        : BaseRepository<TodoListCategory>(dbContext.TodoListCategories, dbContext)
    {
        private List<TodoListCategory> _data;

        protected override IQueryable<TodoListCategory> BaseIncludes(DbSet<TodoListCategory> table)
        {
            return _data.AsQueryable();
        }

        public void Init(List<TodoListCategory> elements)
        {
            _data = elements;
        }
    }
}
