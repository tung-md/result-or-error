using FluentAssertions;
using ResultOrError;

namespace Tests;

public class SwitchTests
{
    private record Person(string Name);

    [Fact]
    public void CallingSwitch_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        void OnValueAction(Person person) => person.Should().BeEquivalentTo(personOrError.Value);
        void OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Action action = () => personOrError.Switch(
            OnValueAction,
            OnErrorsAction);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void CallingSwitch_WhenIsError_ShouldExecuteOnErrorAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new List<Error> { Error.Validation(), Error.Conflict() };
        void OnValueAction(Person _) => throw new Exception("Should not be called");
        void OnErrorsAction(IReadOnlyList<Error> errors) => errors.Should().BeEquivalentTo(personOrError.Errors);

        // Act
        Action action = () => personOrError.Switch(
            OnValueAction,
            OnErrorsAction);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void CallingSwitchFirst_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        void OnValueAction(Person person) => person.Should().BeEquivalentTo(personOrError.Value);
        void OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Action action = () => personOrError.SwitchFirst(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void CallingSwitchFirst_WhenIsError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new List<Error> { Error.Validation(), Error.Conflict() };
        void OnValueAction(Person _) => throw new Exception("Should not be called");
        void OnFirstErrorAction(Error errors)
            => errors.Should().BeEquivalentTo(personOrError.Errors[0])
                .And.BeEquivalentTo(personOrError.FirstError);

        // Act
        Action action = () => personOrError.SwitchFirst(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public async Task CallingSwitchFirstAfterThenAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        void OnValueAction(Person person) => person.Should().BeEquivalentTo(personOrError.Value);
        void OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = () => personOrError
            .ThenAsync(person => Task.FromResult(person))
            .SwitchFirst(OnValueAction, OnFirstErrorAction);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task CallingSwitchAfterThenAsync_WhenIsSuccess_ShouldExecuteOnValueAction()
    {
        // Arrange
        ResultOrError<Person> personOrError = new Person("Tung");
        void OnValueAction(Person person) => person.Should().BeEquivalentTo(personOrError.Value);
        void OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = () => personOrError
            .ThenAsync(person => Task.FromResult(person))
            .Switch(OnValueAction, OnErrorsAction);

        // Assert
        await action.Should().NotThrowAsync();
    }
}
