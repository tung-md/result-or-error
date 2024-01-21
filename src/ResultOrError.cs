namespace ResultOrError;

/// <summary>
/// A discriminated union of errors or a value.
/// </summary>
public readonly record struct ResultOrError<TValue> : IResultOrError<TValue>
{
    private readonly TValue? _value = default;
    private readonly List<Error>? _errors = null;

    private static readonly Error NoFirstError = Error.Unexpected(
        code: "ResultOrError.NoFirstError",
        description: "First error cannot be retrieved from a successful ResultOrError.");

    private static readonly Error NoErrors = Error.Unexpected(
        code: "ResultOrError.NoErrors",
        description: "Error list cannot be retrieved from a successful ResultOrError.");

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    public bool IsError { get; }

    /// <summary>
    /// Gets the list of errors. If the state is not error, the list will contain a single error representing the state.
    /// </summary>
    public List<Error> Errors => IsError ? _errors! : new List<Error> { NoErrors };

    /// <summary>
    /// Gets the list of errors. If the state is not error, the list will be empty.
    /// </summary>
    public List<Error> ErrorsOrEmptyList => IsError ? _errors! : new();

    /// <summary>
    /// Creates an <see cref="ResultOrError{TValue}"/> from a list of errors.
    /// </summary>
    public static ResultOrError<TValue> From(List<Error> errors)
    {
        return errors;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public TValue Value => _value!;

    /// <summary>
    /// Gets the first error.
    /// </summary>
    public Error FirstError
    {
        get
        {
            if (!IsError)
            {
                return NoFirstError;
            }

            return _errors![0];
        }
    }

    private ResultOrError(Error error)
    {
        _errors = new List<Error> { error };
        IsError = true;
    }

    private ResultOrError(List<Error> errors)
    {
        _errors = errors;
        IsError = true;
    }

    private ResultOrError(TValue value)
    {
        _value = value;
        IsError = false;
    }

    /// <summary>
    /// Creates an <see cref="ResultOrError{TValue}"/> from a value.
    /// </summary>
    public static implicit operator ResultOrError<TValue>(TValue value)
    {
        return new ResultOrError<TValue>(value);
    }

    /// <summary>
    /// Creates an <see cref="ResultOrError{TValue}"/> from an error.
    /// </summary>
    public static implicit operator ResultOrError<TValue>(Error error)
    {
        return new ResultOrError<TValue>(error);
    }

    /// <summary>
    /// Creates an <see cref="ResultOrError{TValue}"/> from a list of errors.
    /// </summary>
    public static implicit operator ResultOrError<TValue>(List<Error> errors)
    {
        return new ResultOrError<TValue>(errors);
    }

    /// <summary>
    /// Creates an <see cref="ResultOrError{TValue}"/> from a list of errors.
    /// </summary>
    public static implicit operator ResultOrError<TValue>(Error[] errors)
    {
        return new ResultOrError<TValue>(errors.ToList());
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ResultOrError{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onError">The action to execute if the state is an error.</param>
    public void Switch(Action<TValue> onValue, Action<List<Error>> onError)
    {
        if (IsError)
        {
            onError(Errors);
            return;
        }

        onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="ResultOrError{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onError"/> is executed asynchronously.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed asynchronously.
    /// </summary>
    /// <param name="onValue">The asynchronous action to execute if the state is a value.</param>
    /// <param name="onError">The asynchronous action to execute if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchAsync(Func<TValue, Task> onValue, Func<List<Error>, Task> onError)
    {
        if (IsError)
        {
            await onError(Errors).ConfigureAwait(false);
            return;
        }

        await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the appropriate action based on the state of the <see cref="ResultOrError{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onFirstError"/> is executed using the first error as input.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed.
    /// </summary>
    /// <param name="onValue">The action to execute if the state is a value.</param>
    /// <param name="onFirstError">The action to execute with the first error if the state is an error.</param>
    public void SwitchFirst(Action<TValue> onValue, Action<Error> onFirstError)
    {
        if (IsError)
        {
            onFirstError(FirstError);
            return;
        }

        onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the state of the <see cref="ResultOrError{TValue}"/>.
    /// If the state is an error, the provided action <paramref name="onFirstError"/> is executed asynchronously using the first error as input.
    /// If the state is a value, the provided action <paramref name="onValue"/> is executed asynchronously.
    /// </summary>
    /// <param name="onValue">The asynchronous action to execute if the state is a value.</param>
    /// <param name="onFirstError">The asynchronous action to execute with the first error if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SwitchFirstAsync(Func<TValue, Task> onValue, Func<Error, Task> onFirstError)
    {
        if (IsError)
        {
            await onFirstError(FirstError).ConfigureAwait(false);
            return;
        }

        await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="ResultOrError{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onError">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public TNextValue Match<TNextValue>(Func<TValue, TNextValue> onValue, Func<List<Error>, TNextValue> onError)
    {
        if (IsError)
        {
            return onError(Errors);
        }

        return onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="ResultOrError{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// If the state is an error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The asynchronous function to execute if the state is a value.</param>
    /// <param name="onError">The asynchronous function to execute if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation that yields the result of the executed function.</returns>
    public async Task<TNextValue> MatchAsync<TNextValue>(Func<TValue, Task<TNextValue>> onValue, Func<List<Error>, Task<TNextValue>> onError)
    {
        if (IsError)
        {
            return await onError(Errors).ConfigureAwait(false);
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="ResultOrError{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// If the state is an error, the provided function <paramref name="onFirstError"/> is executed using the first error, and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <param name="onFirstError">The function to execute with the first error if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public TNextValue MatchFirst<TNextValue>(Func<TValue, TNextValue> onValue, Func<Error, TNextValue> onFirstError)
    {
        if (IsError)
        {
            return onFirstError(FirstError);
        }

        return onValue(Value);
    }

    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="ResultOrError{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// If the state is an error, the provided function <paramref name="onFirstError"/> is executed asynchronously using the first error, and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The asynchronous function to execute if the state is a value.</param>
    /// <param name="onFirstError">The asynchronous function to execute with the first error if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation that yields the result of the executed function.</returns>
    public async Task<TNextValue> MatchFirstAsync<TNextValue>(Func<TValue, Task<TNextValue>> onValue, Func<Error, Task<TNextValue>> onFirstError)
    {
        if (IsError)
        {
            return await onFirstError(FirstError).ConfigureAwait(false);
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public ResultOrError<TNextValue> Then<TNextValue>(Func<TValue, ResultOrError<TNextValue>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return onValue(Value);
    }

    /// <summary>
    /// If the state is a value, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <see cref="ResultOrError"/> instance.</returns>
    public ResultOrError<TValue> Then(Action<TValue> action)
    {
        if (IsError)
        {
            return Errors;
        }

        action(Value);

        return this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public ResultOrError<TNextValue> Then<TNextValue>(Func<TValue, TNextValue> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return onValue(Value);
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public async Task<ResultOrError<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<ResultOrError<TNextValue>>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is a value, the provided <paramref name="action"/> is invoked asynchronously.
    /// </summary>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <see cref="ResultOrError"/> instance.</returns>
    public async Task<ResultOrError<TValue>> ThenAsync(Func<TValue, Task> action)
    {
        if (IsError)
        {
            return Errors;
        }

        await action(Value).ConfigureAwait(false);

        return this;
    }

    /// <summary>
    /// If the state is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the result.</typeparam>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original <see cref="Errors"/>.</returns>
    public async Task<ResultOrError<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<TNextValue>> onValue)
    {
        if (IsError)
        {
            return Errors;
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ResultOrError<TValue> Else(Func<List<Error>, Error> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ResultOrError<TValue> Else(Func<List<Error>, List<Error>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is returned.
    /// </summary>
    /// <param name="error">The error to return.</param>
    /// <returns>The given <paramref name="error"/>.</returns>
    public ResultOrError<TValue> Else(Error error)
    {
        if (!IsError)
        {
            return Value;
        }

        return error;
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ResultOrError<TValue> Else(Func<List<Error>, TValue> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return onError(Errors);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed and its result is returned.
    /// </summary>
    /// <param name="onError">The value to return if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public ResultOrError<TValue> Else(TValue onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return onError;
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ResultOrError<TValue>> ElseAsync(Func<List<Error>, Task<TValue>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ResultOrError<TValue>> ElseAsync(Func<List<Error>, Task<Error>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ResultOrError<TValue>> ElseAsync(Func<List<Error>, Task<List<Error>>> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return await onError(Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is awaited and returned.
    /// </summary>
    /// <param name="error">The error to return if the state is error.</param>
    /// <returns>The result from awaiting the given <paramref name="error"/>.</returns>
    public async Task<ResultOrError<TValue>> ElseAsync(Task<Error> error)
    {
        if (!IsError)
        {
            return Value;
        }

        return await error.ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original <see cref="Value"/>.</returns>
    public async Task<ResultOrError<TValue>> ElseAsync(Task<TValue> onError)
    {
        if (!IsError)
        {
            return Value;
        }

        return await onError.ConfigureAwait(false);
    }
}
