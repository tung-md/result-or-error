<div align="center">

<img src="assets/logo.png" alt="drawing" width="700px"/></br>

[![ResultOrError - NuGet](https://img.shields.io/nuget/v/resultorerror.svg?label=ResultOrError%20-%20nuget)](https://www.nuget.org/packages/resultorerror) [![NuGet](https://img.shields.io/nuget/dt/resultorerror.svg)](https://www.nuget.org/packages/resultorerror) [![Build Status](../../actions/workflows/build.yml/badge.svg)](../../actions/workflows/build.yml)

![en](https://img.shields.io/badge/lang-en-green.svg)
[![vi](https://img.shields.io/badge/lang-vi-red.svg)](README.vi-VN.md)
---

### A simple, fluent discriminated union of a result or an error.

`dotnet add package ResultOrError`

</div>

- [Getting Started 🏃](#getting-started-)
  - [Replace throwing exceptions with `ResultOrError<T>`](#replace-throwing-exceptions-with-resultorerrort)
  - [Return Multiple Errors When Needed](#return-multiple-errors-when-needed)
- [Creating an `ResultOrError` instance](#creating-an-resultorerror-instance)
  - [Using implicit conversion](#using-implicit-conversion)
  - [Using The `ResultOrErrorFactory`](#using-the-resultorerrorfactory)
  - [Using The `ToResultOrError` Extension Method](#using-the-toresultorerror-extension-method)
- [Properties](#properties)
  - [`IsError`](#iserror)
  - [`Value`](#value)
  - [`Errors`](#errors)
  - [`FirstError`](#firsterror)
  - [`ErrorsOrEmptyList`](#errorsoremptylist)
- [Methods](#methods)
  - [`Match`](#match)
    - [`Match`](#match-1)
    - [`MatchAsync`](#matchasync)
    - [`MatchFirst`](#matchfirst)
    - [`MatchFirstAsync`](#matchfirstasync)
  - [`Switch`](#switch)
    - [`Switch`](#switch-1)
    - [`SwitchAsync`](#switchasync)
    - [`SwitchFirst`](#switchfirst)
    - [`SwitchFirstAsync`](#switchfirstasync)
  - [`Then`](#then)
    - [`Then`](#then-1)
    - [`ThenAsync`](#thenasync)
    - [Mixing `Then` and `ThenAsync`](#mixing-then-and-thenasync)
  - [`Else`](#else)
    - [`Else`](#else-1)
    - [`ElseAsync`](#elseasync)
- [Mixing Features (`Then`, `Else`, `Switch`, `Match`)](#mixing-features-then-else-switch-match)
- [Error Types](#error-types)
  - [Built in error types](#built-in-error-types)
  - [Custom error types](#custom-error-types)
- [Built in result types (`Result.Success`, ..)](#built-in-result-types-resultsuccess-)
- [Organizing Errors](#organizing-errors)
- [Mediator + FluentValidation + `ResultOrError` 🤝](#mediator--fluentvalidation--resultorerror-)
- [Contribution 🤲](#contribution-)
- [License 🪪](#license-)

# Getting Started 🏃

## Replace throwing exceptions with `ResultOrError<T>`

This 👇

```cs
public float Divide(int a, int b)
{
    if (b == 0)
    {
        throw new Exception("Cannot divide by zero");
    }

    return a / b;
}

try
{
    var result = Divide(4, 2);
    Console.WriteLine(result * 2); // 4
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    return;
}
```

Turns into this 👇

```cs
public ResultOrError<float> Divide(int a, int b)
{
    if (b == 0)
    {
        return Error.Unexpected(description: "Cannot divide by zero");
    }

    return a / b;
}

var result = Divide(4, 2);

if (result.IsError)
{
    Console.WriteLine(result.FirstError.Description);
    return;
}

Console.WriteLine(result.Value * 2); // 4
```

Or, using [Then](#then--thenasync)/[Else](#else--elseasync) and [Switch](#switch--switchasync)/[Match](#match--matchasync), you can do this 👇

```cs

Divide(4, 2)
    .Then(val => val * 2)
    .SwitchFirst(
        onValue: Console.WriteLine, // 4
        onFirstError: error => Console.WriteLine(error.Description));
```

## Return Multiple Errors When Needed

Internally, the `ResultOrError` object has a list of `Error`s, so if you have multiple errors, you don't need to compromise and have only the first one.

```cs
public class User(string _name)
{
    public static ResultOrError<User> Create(string name)
    {
        List<Error> errors = [];

        if (name.Length < 2)
        {
            errors.Add(Error.Validation(description: "Name is too short"));
        }

        if (name.Length > 100)
        {
            errors.Add(Error.Validation(description: "Name is too long"));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(Error.Validation(description: "Name cannot be empty or whitespace only"));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new User(name);
    }
}
```

# Creating an `ResultOrError` instance

## Using implicit conversion

There are implicit converters from `TResult`, `Error`, `List<Error>` to `ResultOrError<TResult>`

```cs
ResultOrError<int> result = 5;
ResultOrError<int> result = Error.Unexpected();
ResultOrError<int> result = [Error.Validation(), Error.Validation()];
```

```cs
public ResultOrError<int> IntToResultOrError()
{
    return 5;
}
```

```cs
public ResultOrError<int> SingleErrorToResultOrError()
{
    return Error.Unexpected();
}
```

```cs
public ResultOrError<int> MultipleErrorsToResultOrError()
{
    return [
        Error.Validation(description: "Invalid Name"),
        Error.Validation(description: "Invalid Last Name")
    ];
}
```

## Using The `ResultOrErrorFactory`

```cs
ResultOrError<int> result = ResultOrErrorFactory.From(5);
ResultOrError<int> result = ResultOrErrorFactory.From<int>(Error.Unexpected());
ResultOrError<int> result = ResultOrErrorFactory.From<int>([Error.Validation(), Error.Validation()]);
```

```cs
public ResultOrError<int> GetValue()
{
    return ResultOrErrorFactory.From(5);
}
```

```cs
public ResultOrError<int> SingleErrorToResultOrError()
{
    return ResultOrErrorFactory.From<int>(Error.Unexpected());
}
```

```cs
public ResultOrError<int> MultipleErrorsToResultOrError()
{
    return ResultOrErrorFactory.From([
        Error.Validation(description: "Invalid Name"),
        Error.Validation(description: "Invalid Last Name")
    ]);
}
```

## Using The `ToResultOrError` Extension Method

```cs
ResultOrError<int> result = 5.ToResultOrError();
ResultOrError<int> result = Error.Unexpected().ToResultOrError<int>();
ResultOrError<int> result = new[] { Error.Validation(), Error.Validation() }.ToResultOrError<int>();
```

# Properties

## `IsError`

```cs
ResultOrError<int> result = User.Create();

if (result.IsError)
{
    // the result contains one or more errors
}
```

## `Value`

```cs
ResultOrError<int> result = User.Create();

if (!result.IsError) // the result contains a value
{
    Console.WriteLine(result.Value);
}
```

## `Errors`

```cs
ResultOrError<int> result = User.Create();

if (result.IsError)
{
    result.Errors // contains the list of errors that occurred
        .ForEach(error => Console.WriteLine(error.Description));
}
```

## `FirstError`

```cs
ResultOrError<int> result = User.Create();

if (result.IsError)
{
    var firstError = result.FirstError; // only the first error that occurred
    Console.WriteLine(firstError == result.Errors[0]); // true
}
```

## `ErrorsOrEmptyList`

```cs
ResultOrError<int> result = User.Create();

if (result.IsError)
{
    result.ErrorsOrEmptyList // List<Error> { /* one or more errors */  }
    return;
}

result.ErrorsOrEmptyList // List<Error> { }
```

# Methods

## `Match`

The `Match` method receives two functions, `onValue` and `onError`, `onValue` will be invoked if the result is success, and `onError` is invoked if the result is an error.

### `Match`

```cs
string foo = result.Match(
    value => value,
    errors => $"{errors.Count} errors occurred.");
```

### `MatchAsync`

```cs
string foo = await result.MatchAsync(
    value => Task.FromResult(value),
    errors => Task.FromResult($"{errors.Count} errors occurred."));
```

### `MatchFirst`

The `MatchFirst` method receives two functions, `onValue` and `onError`, `onValue` will be invoked if the result is success, and `onError` is invoked if the result is an error.

Unlike `Match`, if the state is error, `MatchFirst`'s `onError` function receives only the first error that occurred, not the entire list of errors.


```cs
string foo = result.MatchFirst(
    value => value,
    firstError => firstError.Description);
```

### `MatchFirstAsync`

```cs
string foo = await result.MatchFirstAsync(
    value => Task.FromResult(value),
    firstError => Task.FromResult(firstError.Description));
```

## `Switch`

The `Switch` method receives two actions, `onValue` and `onError`, `onValue` will be invoked if the result is success, and `onError` is invoked if the result is an error.

### `Switch`

```cs
result.Switch(
    value => Console.WriteLine(value),
    errors => Console.WriteLine($"{errors.Count} errors occurred."));
```

### `SwitchAsync`

```cs
await result.SwitchAsync(
    value => { Console.WriteLine(value); return Task.CompletedTask; },
    errors => { Console.WriteLine($"{errors.Count} errors occurred."); return Task.CompletedTask; });
```

### `SwitchFirst`

The `SwitchFirst` method receives two actions, `onValue` and `onError`, `onValue` will be invoked if the result is success, and `onError` is invoked if the result is an error.

Unlike `Switch`, if the state is error, `SwitchFirst`'s `onError` function receives only the first error that occurred, not the entire list of errors.

```cs
result.SwitchFirst(
    value => Console.WriteLine(value),
    firstError => Console.WriteLine(firstError.Description));
```

###  `SwitchFirstAsync`

```cs
await result.SwitchFirstAsync(
    value => { Console.WriteLine(value); return Task.CompletedTask; },
    firstError => { Console.WriteLine(firstError.Description); return Task.CompletedTask; });
```

## `Then`

### `Then`

`Then` receives an action or a function, and invokes it only if the result is not an error.

```cs
ResultOrError<int> foo = result
    .Then(val => val * 2);
```

Multiple `Then` methods can be chained together.

```cs
ResultOrError<string> foo = result
    .Then(val => val * 2)
    .Then(val => $"The result is {val}");
```

If any of the methods return an error, the chain will break and the errors will be returned.

```cs
ResultOrError<int> Foo() => Error.Unexpected();

ResultOrError<string> foo = result
    .Then(val => val * 2)
    .Then(_ => GetAnError())
    .Then(val => $"The result is {val}") // this function will not be invoked
    .Then(val => $"The result is {val}"); // this function will not be invoked
```

### `ThenAsync`

`ThenAsync` receives an asynchronous action or function, and invokes it only if the result is not an error.

```cs
ResultOrError<string> foo = await result
    .ThenAsync(val => Task.Delay(val))
    .ThenAsync(val => Task.FromResult($"The result is {val}"));
```

### Mixing `Then` and `ThenAsync`

You can mix `Then` and `ThenAsync` methods together.

```cs
ResultOrError<string> foo = await result
    .ThenAsync(val => Task.Delay(val))
    .Then(val => Console.WriteLine($"Finsihed waiting {val} seconds."))
    .ThenAsync(val => Task.FromResult(val * 2))
    .Then(val => $"The result is {val}");
```

## `Else`

`Else` receives a value or a function. If the result is an error, `Else` will return the value or invoke the function. Otherwise, it will return the value of the result.

### `Else`

```cs
ResultOrError<string> foo = result
    .Else("fallback value");
```

```cs
ResultOrError<string> foo = result
    .Else(errors => $"{errors.Count} errors occurred.");
```

### `ElseAsync`

```cs
ResultOrError<string> foo = await result
    .ElseAsync(Task.FromResult("fallback value"));
```

```cs
ResultOrError<string> foo = await result
    .ElseAsync(errors => Task.FromResult($"{errors.Count} errors occurred."));
```

# Mixing Features (`Then`, `Else`, `Switch`, `Match`)

You can mix `Then`, `Else`, `Switch` and `Match` methods together.

```cs
ResultOrError<string> foo = await result
    .ThenAsync(val => Task.Delay(val))
    .Then(val => Console.WriteLine($"Finsihed waiting {val} seconds."))
    .ThenAsync(val => Task.FromResult(val * 2))
    .Then(val => $"The result is {val}")
    .Else(errors => Error.Unexpected())
    .MatchFirst(
        value => value,
        firstError => $"An error occurred: {firstError.Description}");
```

# Error Types

Each `Error` instance has a `Type` property, which is an enum value that represents the type of the error.

## Built in error types

The following error types are built in:

```cs
public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
}
```

Each error type has a static method that creates an error of that type. For example:

```cs
var error = Error.NotFound();
```

optionally, you can pass a code, description and metadata to the error:

```cs
var error = Error.Unexpected(
    code: "User.ShouldNeverHappen",
    description: "A user error that should never happen",
    metadata: new Dictionary<string, object>
    {
        { "user", user },
    });
```
The `ErrorType` enum is a good way to categorize errors.

## Custom error types

You can create your own error types if you would like to categorize your errors differently.

A custom error type can be created with the `Custom` static method

```cs
public static class MyErrorTypes
{
    const int ShouldNeverHappen = 12;
}

var error = Error.Custom(
    type: MyErrorTypes.ShouldNeverHappen,
    code: "User.ShouldNeverHappen",
    description: "A user error that should never happen");
```

You can use the `Error.NumericType` method to retrieve the numeric type of the error.

```cs
var errorMessage = Error.NumericType switch
{
    MyErrorType.ShouldNeverHappen => "Consider replacing dev team",
    _ => "An unknown error occurred.",
};
```

# Built in result types (`Result.Success`, ..)

There are a few built in result types:

```cs
ResultOrError<Success> result = Result.Success;
ResultOrError<Created> result = Result.Created;
ResultOrError<Updated> result = Result.Updated;
ResultOrError<Deleted> result = Result.Deleted;
```

Which can be used as following

```cs
ResultOrError<Deleted> DeleteUser(Guid id)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user is null)
    {
        return Error.NotFound(description: "User not found.");
    }

    await _userRepository.DeleteAsync(user);
    return Result.Deleted;
}
```

# Organizing Errors

A nice approach, is creating a static class with the expected errors. For example:

```cs
public static partial class DivisionErrors
{
    public static Error CannotDivideByZero = Error.Unexpected(
        code: "Division.CannotDivideByZero",
        description: "Cannot divide by zero.");
}
```

Which can later be used as following 👇

```cs
public ResultOrError<float> Divide(int a, int b)
{
    if (b == 0)
    {
        return DivisionErrors.CannotDivideByZero;
    }

    return a / b;
}
```

# [Mediator](https://github.com/jbogard/MediatR) + [FluentValidation](https://github.com/FluentValidation/FluentValidation) + `ResultOrError` 🤝

A common approach when using `MediatR` is to use `FluentValidation` to validate the request before it reaches the handler.

Usually, the validation is done using a `Behavior` that throws an exception if the request is invalid.

Using `ResultOrError`, we can create a `Behavior` that returns an error instead of throwing an exception.

This plays nicely when the project uses `ResultOrError`, as the layer invoking the `Mediator`, similar to other components in the project, simply receives an `ResultOrError` and can handle it accordingly.

Here is an example of a `Behavior` that validates the request and returns an error if it's invalid 👇

```cs
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResultOrError
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(
                code: error.PropertyName,
                description: error.ErrorMessage));

        return (dynamic)errors;
    }
}
```

# Contribution 🤲

If you have any questions, comments, or suggestions, please open an issue or create a pull request 🙂

# License 🪪

This project is licensed under the terms of the [MIT](https://github.com/tung-md/result-or-error/blob/main/LICENSE) license.
