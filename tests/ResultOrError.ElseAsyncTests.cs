using FluentAssertions;
using ResultOrError;

namespace Tests;

public class ElseAsyncTests
{
    [Fact]
    public async Task CallingElseAsyncWithValueFunc_WhenIsSuccess_ShouldNotInvokeElseFunc()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(errors => Task.FromResult($"Error count: {errors.Count}"));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(stringOrError.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithValueFunc_WhenIsError_ShouldInvokeElseFunc()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(errors => Task.FromResult($"Error count: {errors.Count}"));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("Error count: 1");
    }

    [Fact]
    public async Task CallingElseAsyncWithValue_WhenIsSuccess_ShouldNotReturnElseValue()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(Task.FromResult("oh no"));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(stringOrError.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithValue_WhenIsError_ShouldReturnElseValue()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(Task.FromResult("oh no"));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("oh no");
    }

    [Fact]
    public async Task CallingElseAsyncWithError_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(Task.FromResult(Error.Unexpected()));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public async Task CallingElseAsyncWithError_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(Task.FromResult(Error.Unexpected()));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(stringOrError.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithErrorFunc_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(errors => Task.FromResult(Error.Unexpected()));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public async Task CallingElseAsyncWithErrorFunc_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(errors => Task.FromResult(Error.Unexpected()));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(stringOrError.Value);
    }

    [Fact]
    public async Task CallingElseAsyncWithErrorFunc_WhenIsError_ShouldReturnElseErrors()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(errors => Task.FromResult(new List<Error> { Error.Unexpected() }));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public async Task CallingElseAsyncWithErrorFunc_WhenIsSuccess_ShouldNotReturnElseErrors()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .ElseAsync(errors => Task.FromResult(new List<Error> { Error.Unexpected() }));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(stringOrError.Value);
    }

    private static Task<ResultOrError<string>> ConvertToStringAsync(int num) => Task.FromResult(ResultOrErrorFactory.From(num.ToString()));

    private static Task<ResultOrError<int>> ConvertToIntAsync(string str) => Task.FromResult(ResultOrErrorFactory.From(int.Parse(str)));
}
