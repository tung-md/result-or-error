namespace ResultOrError;

/// <summary>
/// Provides factory methods for creating instances of <see cref="ResultOrError{TValue}"/>.
/// </summary>
public static class ResultOrErrorFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="ResultOrError{TValue}"/> with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An instance of <see cref="ResultOrError{TValue}"/> containing the provided value.</returns>
    public static ResultOrError<TValue> From<TValue>(TValue value)
    {
        return value;
    }
}
