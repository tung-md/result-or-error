using FluentAssertions;
using ResultOrError;

namespace Tests;

public class ElseTests
{
    [Fact]
    public void CallingElseWithValueFunc_WhenIsSuccess_ShouldNotInvokeElseFunc()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else(errors => $"Error count: {errors.Count}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(stringOrError.Value);
    }

    [Fact]
    public void CallingElseWithValueFunc_WhenIsError_ShouldReturnElseValue()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else(errors => $"Error count: {errors.Count}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("Error count: 1");
    }

    [Fact]
    public void CallingElseWithValue_WhenIsSuccess_ShouldNotReturnElseValue()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else("oh no");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(stringOrError.Value);
    }

    [Fact]
    public void CallingElseWithValue_WhenIsError_ShouldReturnElseValue()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else("oh no");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("oh no");
    }

    [Fact]
    public void CallingElseWithError_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else(Error.Unexpected());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CallingElseWithError_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else(Error.Unexpected());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(stringOrError.Value);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else(errors => Error.Unexpected());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsSuccess_ShouldNotReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else(errors => Error.Unexpected());

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(stringOrError.Value);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsError_ShouldReturnElseErrors()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else(errors => new() { Error.Unexpected() });

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CallingElseWithErrorsFunc_WhenIsSuccess_ShouldNotReturnElseErrors()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => ConvertToString(num))
            .Else(errors => new() { Error.Unexpected() });

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(stringOrError.Value);
    }

    [Fact]
    public async Task CallingElseWithValueAfterThenAsync_WhenIsError_ShouldReturnElseValue()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .Then(str => ConvertToInt(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .Else("oh no");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("oh no");
    }

    [Fact]
    public async Task CallingElseWithValueFuncAfterThenAsync_WhenIsError_ShouldReturnElseValue()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .Then(str => ConvertToInt(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .Else(errors => $"Error count: {errors.Count}");

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("Error count: 1");
    }

    [Fact]
    public async Task CallingElseWithErrorAfterThenAsync_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .Then(str => ConvertToInt(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .Else(Error.Unexpected());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public async Task CallingElseWithErrorFuncAfterThenAsync_WhenIsError_ShouldReturnElseError()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .Then(str => ConvertToInt(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .Else(errors => Error.Unexpected());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public async Task CallingElseWithErrorFuncAfterThenAsync_WhenIsError_ShouldReturnElseErrors()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .Then(str => ConvertToInt(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .Else(errors => new() { Error.Unexpected() });

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }

    private static ResultOrError<string> ConvertToString(int num) => num.ToString();

    private static ResultOrError<int> ConvertToInt(string str) => int.Parse(str);

    private static Task<ResultOrError<string>> ConvertToStringAsync(int num) => Task.FromResult(ResultOrErrorFactory.From(num.ToString()));
}
