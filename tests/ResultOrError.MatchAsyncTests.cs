using FluentAssertions;
using ResultOrError;

namespace Tests;

public class MatchAsyncTests
{
    private record Person(string Name);

    [Fact]
    public async Task CallingMatchAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        Task<string> OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(personOrError.Value);
            return Task.FromResult("Nice");
        }

        Task<string> OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task<string>> action = async () => await personOrError.MatchAsync(
            OnValueAction,
            OnErrorsAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task CallingMatchAsync_WhenIsError_ShouldExecuteOnErrorAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> OnValueAction(Person _) => throw new Exception("Should not be called");

        Task<string> OnErrorsAction(IReadOnlyList<Error> errors)
        {
            errors.Should().BeEquivalentTo(personOrError.Errors);
            return Task.FromResult("Nice");
        }

        // Act
        Func<Task<string>> action = async () => await personOrError.MatchAsync(
            OnValueAction,
            OnErrorsAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task CallingMatchFirstAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        Task<string> OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(personOrError.Value);
            return Task.FromResult("Nice");
        }

        Task<string> OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task<string>> action = async () => await personOrError.MatchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task CallingMatchFirstAsync_WhenIsError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> OnValueAction(Person _) => throw new Exception("Should not be called");
        Task<string> OnFirstErrorAction(Error errors)
        {
            errors.Should().BeEquivalentTo(personOrError.Errors[0])
                .And.BeEquivalentTo(personOrError.FirstError);

            return Task.FromResult("Nice");
        }

        // Act
        Func<Task<string>> action = async () => await personOrError.MatchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task CallingMatchFirstAsyncAfterThenAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> OnValueAction(Person _) => throw new Exception("Should not be called");
        Task<string> OnFirstErrorAction(Error errors)
        {
            errors.Should().BeEquivalentTo(personOrError.Errors[0])
                .And.BeEquivalentTo(personOrError.FirstError);

            return Task.FromResult("Nice");
        }

        // Act
        Func<Task<string>> action = () => personOrError
            .ThenAsync(person => Task.FromResult(person))
            .MatchFirstAsync(OnValueAction, OnFirstErrorAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task CallingMatchAsyncAfterThenAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> OnValueAction(Person _) => throw new Exception("Should not be called");

        Task<string> OnErrorsAction(IReadOnlyList<Error> errors)
        {
            errors.Should().BeEquivalentTo(personOrError.Errors);
            return Task.FromResult("Nice");
        }

        // Act
        Func<Task<string>> action = () => personOrError
            .ThenAsync(person => Task.FromResult(person))
            .MatchAsync(OnValueAction, OnErrorsAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }
}
