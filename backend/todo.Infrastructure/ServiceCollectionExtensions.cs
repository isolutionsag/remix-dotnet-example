using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using todo.Core.Repositories;
using todo.Infrastructure.Repositories;

namespace todo.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddAndAutoMigrateDb(this IServiceCollection services)
    {
        services.AddDbContext<ITodoContext, TodoContextSqlLite>();
        services.AddTransient<ITodoListCategoryRepository, TodoListCategoryRepository>();
        services.AddTransient<ITodoListRepository, TodoListRepository>();
        services.AddTransient<ITodoListEntryRepository, TodoListEntryRepository>();

        // migrate and initialize db
        using var scope = services.BuildServiceProvider().CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TodoContextSqlLite>();
        db.Database.Migrate();
        DbInitializer.Init(db);
    }
}