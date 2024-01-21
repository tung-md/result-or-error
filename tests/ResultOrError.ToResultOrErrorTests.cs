using FluentAssertions;
using ResultOrError;

namespace Tests;

public class ToResultOrErrorTests
{
    [Fact]
    public void ValueToResultOrError_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        int value = 5;

        // Act
        ResultOrError<int> result = value.ToResultOrError();

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void ErrorToResultOrError_WhenAccessingFirstError_ShouldReturnSameError()
    {
        // Arrange
        Error error = Error.Unauthorized();

        // Act
        ResultOrError<int> result = error.ToResultOrError<int>();

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public void ListOfErrorsToResultOrError_WhenAccessingErrors_ShouldReturnSameErrors()
    {
        // Arrange
        List<Error> errors = new List<Error> { Error.Unauthorized(), Error.Validation() };

        // Act
        ResultOrError<int> result = errors.ToResultOrError<int>();

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }
}
