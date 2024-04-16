using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using todo.Core.Dtos;

namespace todo.Core.Tests;

public class DomainOperationResultTests
{

    [Fact]
    public void Success_ResultIsSet_ReturnsSuccess()
    {
        // arrange
        var expectedResult = "Hello, World!";
        var result = DomainOperationResult<string>.Success(expectedResult);

        // act
        var isSuccess = result.IsSuccess();

        // assert
        isSuccess.Should().BeTrue();
        result.Result.Should().Be(expectedResult);
    }

    [Fact]
    public void Success_ValidationFailuresIsEmpty_ReturnsSuccess()
    {
        // arrange
        var result = DomainOperationResult<int>.Success(42);

        // act
        var isSuccess = result.IsSuccess();

        // assert
        isSuccess.Should().BeTrue();
        result.Result.Should().Be(42);
    }

    [Fact]
    public void Result_ValidationFailuresNotEmpty_ThrowsInvalidOperationException()
    {
        // arrange
        var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Property1", "Error1"),
                new ValidationFailure("Property2", "Error2")
            };

        var result = new DomainOperationResult<object>(null, validationFailures);

        // act & assert
        var act = () => result.Result;
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ProduceErrorResponse_ValidationFailures_ReturnsBadRequestObjectResult()
    {
        // arrange
        var expectedError1 = (Property: "Property1", Error: "Error1");
        var expectedError2 = (Property: "Property2", Error: "Error2");
        var validationFailures = new List<ValidationFailure>
            {
                new(expectedError1.Property, expectedError1.Error),
                new(expectedError2.Property, expectedError2.Error)
            };
        var result = new DomainOperationResult<int>(5, validationFailures);

        // act
        var response = result.ProduceErrorResponse();

        // assert
        response.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)response;
        var validationFailureDtos = (List<ValidationFailureDto>)badRequestResult.Value;
        validationFailureDtos.Should().HaveCount(2);
        validationFailureDtos[0].PropertyName.Should().Be(expectedError1.Property);
        validationFailureDtos[0].Message.Should().Be(expectedError1.Error);
        validationFailureDtos[1].PropertyName.Should().Be(expectedError2.Property);
        validationFailureDtos[1].Message.Should().Be(expectedError2.Error);
    }

}

