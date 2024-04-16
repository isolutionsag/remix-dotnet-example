using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using todo.Core.Slices.TodoListCategories;
using todo.Core.Slices.TodoListEntries;
using todo.Core.Slices.TodoLists;

namespace todo.Core
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddCore(this IServiceCollection services)
        {
            services.AddTransient<ICreateTodoList, CreateTodoList>();
            services.AddTransient<IRemoveTodoList, RemoveTodoList>();
            services.AddTransient<IUpdateTodoList, UpdateTodoList>();
            services.AddTransient<IGetAllTodoList, GetAllTodoList>();

            services.AddTransient<ICreateTodoListEntry, CreateTodoListEntry>();
            services.AddTransient<IRemoveTodoListEntry, RemoveTodoListEntry>();
            services.AddTransient<IUpdateTodoListEntry, UpdateTodoListEntry>();
            services.AddTransient<IGetAllTodoListEntry, GetAllTodoListEntry>();

            services.AddTransient<ICreateTodoListCategory, CreateTodoListCategory>();
            services.AddTransient<IRemoveTodoListCategory, RemoveTodoListCategory>();
            services.AddTransient<IUpdateTodoListCategory, UpdateTodoListCategory>();
            services.AddTransient<IGetAllTodoListCategory, GetAllTodoListCategory>();
        }
    }
}
