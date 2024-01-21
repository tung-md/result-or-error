namespace ResultOrError;

public static partial class ResultOrErrorExtensions
{
    /// <summary>
    /// If the state of <paramref name="resultOrError"/> is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<ResultOrError<TNextValue>> Then<TValue, TNextValue>(this Task<ResultOrError<TValue>> resultOrError, Func<TValue, ResultOrError<TNextValue>> onValue)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return result.Then(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="resultOrError"/> is a value, the provided function <paramref name="onValue"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<ResultOrError<TNextValue>> Then<TValue, TNextValue>(this Task<ResultOrError<TValue>> resultOrError, Func<TValue, TNextValue> onValue)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return result.Then(onValue);
    }

    /// <summary>
    /// If the state of <paramref name="resultOrError"/> is a value, the provided <paramref name="action"/> is invoked.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <paramref name="resultOrError"/>.</returns>
    public static async Task<ResultOrError<TValue>> Then<TValue>(this Task<ResultOrError<TValue>> resultOrError, Action<TValue> action)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return result.Then(action);
    }

    /// <summary>
    /// If the state of <paramref name="resultOrError"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<ResultOrError<TNextValue>> ThenAsync<TValue, TNextValue>(this Task<ResultOrError<TValue>> resultOrError, Func<TValue, Task<ResultOrError<TNextValue>>> onValue)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return await result.ThenAsync(onValue).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="resultOrError"/> is a value, the provided function <paramref name="onValue"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <typeparam name="TNextValue">The type of the result from invoking the <paramref name="onValue"/> function.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onValue">The function to execute if the state is a value.</param>
    /// <returns>The result from calling <paramref name="onValue"/> if state is value; otherwise the original errors.</returns>
    public static async Task<ResultOrError<TNextValue>> ThenAsync<TValue, TNextValue>(this Task<ResultOrError<TValue>> resultOrError, Func<TValue, Task<TNextValue>> onValue)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return await result.ThenAsync(onValue).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state of <paramref name="resultOrError"/> is a value, the provided <paramref name="action"/> is executed asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="action">The action to execute if the state is a value.</param>
    /// <returns>The original <paramref name="resultOrError"/>.</returns>
    public static async Task<ResultOrError<TValue>> ThenAsync<TValue>(this Task<ResultOrError<TValue>> resultOrError, Func<TValue, Task> action)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return await result.ThenAsync(action).ConfigureAwait(false);
    }
}
