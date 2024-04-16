using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using todo.Core.Dtos;

namespace todo.Core
{
    public class DomainOperationResult<T>
    {
        private readonly T? result;
        public T Result
        {
            get
            {
                return ValidationFailures.Any()
                    ? throw new InvalidOperationException(
                        "Cannot access result of failed operation"
                    )
                    : result!;
            }
        }
        public IEnumerable<ValidationFailure> ValidationFailures { get; }

        public DomainOperationResult(T? result, IEnumerable<ValidationFailure> validationFailures)
        {
            this.result = result;
            ValidationFailures = validationFailures;
        }

        public bool IsSuccess()
        {
            return !ValidationFailures.Any();
        }

        public ActionResult ProduceErrorResponse()
        {
            return new BadRequestObjectResult(
                ValidationFailures
                    .Select(
                        failure =>
                            new ValidationFailureDto(
                                failure.PropertyName,
                                failure.ErrorCode,
                                failure.ErrorMessage
                            )
                    )
                    .ToList()
            );
        }

        public static DomainOperationResult<T> Success(T? result) =>
            new(result, Enumerable.Empty<ValidationFailure>());
    }
}