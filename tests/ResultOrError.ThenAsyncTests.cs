using FluentAssertions;
using ResultOrError;

namespace Tests;

public class ThenAsyncTests
{
    [Fact]
    public async Task CallingThenAsync_WhenIsSuccess_ShouldInvokeNextThen()
    {
        // Arrange
        ResultOrError<string> stringOrError = "5";

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => Task.FromResult(num * 2))
            .ThenAsync(num => Task.Run(() => { _ = 5; }))
            .ThenAsync(num => ConvertToStringAsync(num));

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo("10");
    }

    [Fact]
    public async Task CallingThenAsync_WhenIsError_ShouldReturnErrors()
    {
        // Arrange
        ResultOrError<string> stringOrError = Error.NotFound();

        // Act
        ResultOrError<string> result = await stringOrError
            .ThenAsync(str => ConvertToIntAsync(str))
            .ThenAsync(num => Task.FromResult(num * 2))
            .ThenAsync(num => Task.Run(() => { _ = 5; }))
            .ThenAsync(num => ConvertToStringAsync(num));

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().BeEquivalentTo(stringOrError.FirstError);
    }

    private static Task<ResultOrError<string>> ConvertToStringAsync(int num) => Task.FromResult(ResultOrErrorFactory.From(num.ToString()));

    private static Task<ResultOrError<int>> ConvertToIntAsync(string str) => Task.FromResult(ResultOrErrorFactory.From(int.Parse(str)));
}
