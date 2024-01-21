namespace ResultOrError;

public static partial class ResultOrErrorExtensions
{
    /// <summary>
    /// Creates an <see cref="ResultOrError{TValue}"/> instance with the given <paramref name="value"/>.
    /// </summary>
    public static ResultOrError<TValue> ToResultOrError<TValue>(this TValue value)
    {
        return value;
    }

    /// <summary>
    /// Creates an <see cref="ResultOrError{TValue}"/> instance with the given <paramref name="error"/>.
    /// </summary>
    public static ResultOrError<TValue> ToResultOrError<TValue>(this Error error)
    {
        return error;
    }

    /// <summary>
    /// Creates an <see cref="ResultOrError{TValue}"/> instance with the given <paramref name="error"/>.
    /// </summary>
    public static ResultOrError<TValue> ToResultOrError<TValue>(this List<Error> error)
    {
        return error;
    }
}
