namespace ResultOrError;

public interface IResultOrError<out TValue> : IResultOrError
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    TValue Value { get; }
}

/// <summary>
/// Type-less interface for the <see cref="ResultOrError"/> object.
/// </summary>
/// <remarks>
/// This interface is intended for use when the underlying type of the <see cref="ResultOrError"/> object is unknown.
/// </remarks>
public interface IResultOrError
{
    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    List<Error>? Errors { get; }

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    bool IsError { get; }
}
