namespace Tests;

using ResultOrError;
using FluentAssertions;

public class ResultOrErrorTests
{
    private record Person(string Name);

    [Fact]
    public void CreateFromFactory_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = new[] { "value" };

        // Act
        ResultOrError<IEnumerable<string>> personOrError = ResultOrErrorFactory.From(value);

        // Assert
        personOrError.IsError.Should().BeFalse();
        personOrError.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        // Arrange
        IEnumerable<string> value = new[] { "value" };
        ResultOrError<IEnumerable<string>> personOrError = ResultOrErrorFactory.From(value);

        // Act
        List<Error> errors = personOrError.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<string> value = new[] { "value" };
        ResultOrError<IEnumerable<string>> personOrError = ResultOrErrorFactory.From(value);

        // Act
        List<Error> errors = personOrError.ErrorsOrEmptyList;

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        // Arrange
        IEnumerable<string> value = new[] { "value" };
        ResultOrError<IEnumerable<string>> personOrError = ResultOrErrorFactory.From(value);

        // Act
        Error firstError = personOrError.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = new[] { "value" };

        // Act
        ResultOrError<IEnumerable<string>> personOrError = ResultOrErrorFactory.From(value);

        // Assert
        personOrError.IsError.Should().BeFalse();
        personOrError.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        // Arrange
        IEnumerable<string> value = new[] { "value" };
        ResultOrError<IEnumerable<string>> personOrError = ResultOrErrorFactory.From(value);

        // Act
        List<Error> errors = personOrError.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<string> value = new[] { "value" };
        ResultOrError<IEnumerable<string>> personOrError = ResultOrErrorFactory.From(value);

        // Act
        List<Error> errors = personOrError.ErrorsOrEmptyList;

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateFromValue_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        // Arrange
        IEnumerable<string> value = new[] { "value" };
        ResultOrError<IEnumerable<string>> personOrError = ResultOrErrorFactory.From(value);

        // Act
        Error firstError = personOrError.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        List<Error> errors = new() { Error.Validation("User.Name", "Name is too short") };
        ResultOrError<Person> personOrError = ResultOrError<Person>.From(errors);

        // Act & Assert
        personOrError.IsError.Should().BeTrue();
        personOrError.Errors.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingErrorsOrEmptyList_ShouldReturnErrorList()
    {
        // Arrange
        List<Error> errors = new() { Error.Validation("User.Name", "Name is too short") };
        ResultOrError<Person> personOrError = ResultOrError<Person>.From(errors);

        // Act & Assert
        personOrError.IsError.Should().BeTrue();
        personOrError.ErrorsOrEmptyList.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        List<Error> errors = new() { Error.Validation("User.Name", "Name is too short") };
        ResultOrError<Person> personOrError = ResultOrError<Person>.From(errors);

        // Act
        Person value = personOrError.Value;

        // Assert
        value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        Person result = new Person("Amici");

        // Act
        ResultOrError<Person> resultOrError = result;

        // Assert
        resultOrError.IsError.Should().BeFalse();
        resultOrError.Value.Should().Be(result);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        ResultOrError<Person> personOrError = new Person("Tung");

        // Act
        List<Error> errors = personOrError.Errors;

        // Assert
        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        ResultOrError<Person> personOrError = new Person("Tung");

        // Act
        Error firstError = personOrError.FirstError;

        // Assert
        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastPrimitiveResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        const int result = 4;

        // Act
        ResultOrError<int> resultOrErrorInt = result;

        // Assert
        resultOrErrorInt.IsError.Should().BeFalse();
        resultOrErrorInt.Value.Should().Be(result);
    }

    [Fact]
    public void ImplicitCastResultOrErrorType_WhenAccessingResult_ShouldReturnValue()
    {
        // Act
        ResultOrError<Success> resultOrErrorSuccess = Result.Success;
        ResultOrError<Created> resultOrErrorCreated = Result.Created;
        ResultOrError<Deleted> resultOrErrorDeleted = Result.Deleted;
        ResultOrError<Updated> resultOrErrorUpdated = Result.Updated;

        // Assert
        resultOrErrorSuccess.IsError.Should().BeFalse();
        resultOrErrorSuccess.Value.Should().Be(Result.Success);

        resultOrErrorCreated.IsError.Should().BeFalse();
        resultOrErrorCreated.Value.Should().Be(Result.Created);

        resultOrErrorDeleted.IsError.Should().BeFalse();
        resultOrErrorDeleted.Value.Should().Be(Result.Deleted);

        resultOrErrorUpdated.IsError.Should().BeFalse();
        resultOrErrorUpdated.Value.Should().Be(Result.Updated);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        ResultOrError<Person> personOrError = error;

        // Assert
        personOrError.IsError.Should().BeTrue();
        personOrError.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastError_WhenAccessingValue_ShouldReturnDefault()
    {
        // Arrange
        ResultOrError<Person> personOrError = Error.Validation("User.Name", "Name is too short");

        // Act
        Person value = personOrError.Value;

        // Assert
        value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingFirstError_ShouldReturnError()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        ResultOrError<Person> personOrError = error;

        // Assert
        personOrError.IsError.Should().BeTrue();
        personOrError.FirstError.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        List<Error> errors = new()
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        // Act
        ResultOrError<Person> personOrError = errors;

        // Assert
        personOrError.IsError.Should().BeTrue();
        personOrError.Errors.Should().HaveCount(errors.Count).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingErrors_ShouldReturnErrorArray()
    {
        // Arrange
        Error[] errors = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        // Act
        ResultOrError<Person> personOrError = errors;

        // Assert
        personOrError.IsError.Should().BeTrue();
        personOrError.Errors.Should().HaveCount(errors.Length).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        List<Error> errors = new()
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        // Act
        ResultOrError<Person> personOrError = errors;

        // Assert
        personOrError.IsError.Should().BeTrue();
        personOrError.FirstError.Should().Be(errors[0]);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        Error[] errors = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        // Act
        ResultOrError<Person> personOrError = errors;

        // Assert
        personOrError.IsError.Should().BeTrue();
        personOrError.FirstError.Should().Be(errors[0]);
    }
}
