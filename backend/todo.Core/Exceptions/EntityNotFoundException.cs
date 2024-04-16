using System.Diagnostics.CodeAnalysis;

namespace todo.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class EntityNotFoundException(Guid id, Type type)
        : Exception($"Could not find entity of type={type} and id={id}.")
    {
    }
}
