namespace ResultOrError;

public static partial class ResultOrErrorExtensions
{
    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ResultOrError<TValue>> Else<TValue>(this Task<ResultOrError<TValue>> resultOrError, Func<List<Error>, TValue> onError)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return result.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ResultOrError<TValue>> Else<TValue>(this Task<ResultOrError<TValue>> resultOrError, TValue onError)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return result.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ResultOrError<TValue>> ElseAsync<TValue>(this Task<ResultOrError<TValue>> resultOrError, Func<List<Error>, Task<TValue>> onError)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ResultOrError<TValue>> ElseAsync<TValue>(this Task<ResultOrError<TValue>> resultOrError, Task<TValue> onError)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ResultOrError<TValue>> Else<TValue>(this Task<ResultOrError<TValue>> resultOrError, Func<List<Error>, Error> onError)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return result.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ResultOrError<TValue>> Else<TValue>(this Task<ResultOrError<TValue>> resultOrError, Func<List<Error>, List<Error>> onError)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return result.Else(onError);
    }

    /// <summary>
    /// If the state is error, the provided <paramref name="error"/> is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="error">The error to return.</param>
    /// <returns>The given <paramref name="error"/>.</returns>
    public static async Task<ResultOrError<TValue>> Else<TValue>(this Task<ResultOrError<TValue>> resultOrError, Error error)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return result.Else(error);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ResultOrError<TValue>> ElseAsync<TValue>(this Task<ResultOrError<TValue>> resultOrError, Func<List<Error>, Task<Error>> onError)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ResultOrError<TValue>> ElseAsync<TValue>(this Task<ResultOrError<TValue>> resultOrError, Func<List<Error>, Task<List<Error>>> onError)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }

    /// <summary>
    /// If the state is error, the provided function <paramref name="onError"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TValue">The type of the underlying value in the <paramref name="resultOrError"/>.</typeparam>
    /// <param name="resultOrError">The <see cref="ResultOrError"/> instance.</param>
    /// <param name="onError">The function to execute if the state is error.</param>
    /// <returns>The result from calling <paramref name="onError"/> if state is error; otherwise the original value.</returns>
    public static async Task<ResultOrError<TValue>> ElseAsync<TValue>(this Task<ResultOrError<TValue>> resultOrError, Task<Error> onError)
    {
        var result = await resultOrError.ConfigureAwait(false);

        return await result.ElseAsync(onError).ConfigureAwait(false);
    }
}
