using FluentAssertions;
using ResultOrError;

namespace Tests;

public class ThenTests
{
    [Fact]
    public void CallingThen_WhenIsSuccess_ShouldInvokeGivenFunc()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => num * 2)
            .Then(num => ConvertToString(num));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("10");
    }

    [Fact]
    public void CallingThen_WhenIsSuccess_ShouldInvokeGivenAction()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<int> result = stringOrError
            .Then(str => { _ = 5; })
            .Then(str => ConvertToInt(str))
            .Then(str => { _ = 5; });

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(5);
    }

    [Fact]
    public void CallingThen_WhenIsError_ShouldReturnErrors()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = stringOrError
            .Then(str => ConvertToInt(str))
            .Then(num => num * 2)
            .Then(str => { _ = 5; })
            .Then(num => ConvertToString(num));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().BeEquivalentTo(stringOrError.FirstError);
    }

    [Fact]
    public async Task CallingThenAfterThenAsync_WhenIsSuccess_ShouldInvokeGivenFunc()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .Then(num => num * 2)
            .ThenAsync(num => ConvertToStringAsync(num))
            .Then(str => ConvertToInt(str))
            .ThenAsync(num => ConvertToStringAsync(num))
            .Then(num => { _ = 5; });

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("10");
    }

    [Fact]
    public async Task CallingThenAfterThenAsync_WhenIsError_ShouldReturnErrors()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .Then(num => ConvertToString(num));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().BeEquivalentTo(stringOrError.FirstError);
    }

    private static ResultOrError<string> ConvertToString(int num) => num.ToString();

    private static ResultOrError<int> ConvertToInt(string str) => int.Parse(str);

    private static Task<ResultOrError<int>> ConvertToIntAsync(string str) => Task.FromResult(ResultOrErrorFactory.From(int.Parse(str)));

    private static Task<ResultOrError<string>> ConvertToStringAsync(int num) => Task.FromResult(ResultOrErrorFactory.From(num.ToString()));
}
