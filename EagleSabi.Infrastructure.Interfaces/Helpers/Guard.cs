namespace EagleSabi.Common.Helpers;

public static class Guard
{
    /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null</exception>
    public static T NotNull<T>(T value, string argumentName, string? customMassege = null)
    {
        if (value is null)
            throw new ArgumentNullException(argumentName, customMassege);
        return value;
    }

    /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null</exception>
    /// <exception cref="ArgumentException">if <paramref name="value"/> is empty string</exception>
    public static string NotNullOrEmpty(string? value, string argumentName, string? customMassege = null)
    {
        NotNull(value, argumentName, customMassege);
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException(customMassege ?? $"Argument '{argumentName}' is an empty string", nameof(argumentName));
        return value;
    }

    /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null</exception>
    /// <exception cref="ArgumentException">if <paramref name="value"/> is empty collection</exception>
    public static TCollection NotNullOrEmpty<TItem, TCollection>(TCollection collection, string argumentName, string? customMassege = null) where TCollection : IReadOnlyCollection<TItem>
    {
        NotNull(collection, argumentName, customMassege);
        if (collection.Count <= 0)
            throw new ArgumentException(customMassege ?? $"Argument '{argumentName}' is an empty collection.", argumentName);
        return collection;
    }

    /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null</exception>
    /// <exception cref="ArgumentException">if <paramref name="value"/> is empty collection</exception>
    public static IReadOnlyCollection<T> NotNullOrEmpty<T>(IReadOnlyCollection<T> collection, string argumentName, string? customMassege = null)
    {
        return NotNullOrEmpty<T, IReadOnlyCollection<T>>(collection, argumentName, customMassege);
    }

    /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">if <paramref name="value"/> is less </exception>
    public static T MinimumAndNotNull<T>(T value, T minimum, string argumentName) where T : IComparable
    {
        NotNull(value, argumentName);
        if (value.CompareTo(minimum) < 0)
            throw new ArgumentOutOfRangeException(argumentName, value, $"Argument '{argumentName}' is less than {minimum}.");
        return value;
    }

    /// <exception cref="ArgumentNullException">if <paramref name="value"/> is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">if <paramref name="value"/> is out of range &lt;<paramref name="smallest"/>,<paramref name="maximum"/>&gt;</exception>
    public static T InRangeAndNotNull<T>(T value, T minimum, T maximum, string argumentName) where T : IComparable
    {
        NotNull(value, argumentName);
        if (value.CompareTo(minimum) < 0)
            throw new ArgumentOutOfRangeException(argumentName, value, $"Argument '{argumentName}' is less than {minimum}.");
        if (value.CompareTo(maximum) > 0)
            throw new ArgumentOutOfRangeException(argumentName, value, $"Argument '{argumentName}' is greater than {maximum}.");
        return value;
    }
}