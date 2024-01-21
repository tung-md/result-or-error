using FluentAssertions;
using ResultOrError;

namespace Tests;

public class SwitchAsyncTests
{
    private record Person(string Name);

    [Fact]
    public async Task CallingSwitchAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        Task OnValueAction(Person person) => Task.FromResult(person.Should().BeEquivalentTo(personOrError.Value));
        Task OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = async () => await personOrError.SwitchAsync(
            OnValueAction,
            OnErrorsAction);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task CallingSwitchAsync_WhenIsError_ShouldExecuteOnErrorAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new List<Error> { Error.Validation(), Error.Conflict() };
        Task OnValueAction(Person _) => throw new Exception("Should not be called");
        Task OnErrorsAction(IReadOnlyList<Error> errors) => Task.FromResult(errors.Should().BeEquivalentTo(personOrError.Errors));

        // Act
        Func<Task> action = async () => await personOrError.SwitchAsync(
            OnValueAction,
            OnErrorsAction);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task CallingSwitchFirstAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        Task OnValueAction(Person person) => Task.FromResult(person.Should().BeEquivalentTo(personOrError.Value));
        Task OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = async () => await personOrError.SwitchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task CallingSwitchFirstAsync_WhenIsError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new List<Error> { Error.Validation(), Error.Conflict() };
        Task OnValueAction(Person _) => throw new Exception("Should not be called");
        Task OnFirstErrorAction(Error errors)
            => Task.FromResult(errors.Should().BeEquivalentTo(personOrError.Errors[0])
                .And.BeEquivalentTo(personOrError.FirstError));

        // Act
        Func<Task> action = async () => await personOrError.SwitchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task CallingSwitchFirstAsyncAfterThenAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        Task OnValueAction(Person person) => Task.FromResult(person.Should().BeEquivalentTo(personOrError.Value));
        Task OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = async () => await personOrError
            .ThenAsync(person => Task.FromResult(person))
            .SwitchFirstAsync(
                OnValueAction,
                OnFirstErrorAction);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task CallingSwitchAsyncAfterThenAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        Task OnValueAction(Person person) => Task.FromResult(person.Should().BeEquivalentTo(personOrError.Value));
        Task OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = async () => await personOrError
            .ThenAsync(person => Task.FromResult(person))
            .SwitchAsync(OnValueAction, OnErrorsAction);

        // Assert
        await action.Should().NotThrowAsync();
    }
}
