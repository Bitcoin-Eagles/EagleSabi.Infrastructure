using EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Models;
using EagleSabi.Infrastructure.Common.Helpers;

namespace EagleSabi.Infrastructure.Common.Records.EventSourcing;

public record Error : IError
{
    public string PropertyName { get; init; } = string.Empty;
    public string ErrorMessage { get; init; }
    public Exception? InnerException { get; init; } = null;

    public Error(string errorMessage)
    {
        Guard.NotNull(errorMessage, nameof(errorMessage));
        ErrorMessage = errorMessage;
    }

    public Error(Exception innerException)
        : this((innerException ?? throw new ArgumentNullException(nameof(innerException))).Message)
    {
        InnerException = innerException;
    }

    public Error(string propertyName, string errorMessage) : this(errorMessage)
    {
        Guard.NotNull(propertyName, nameof(propertyName));
        PropertyName = propertyName;
    }

    public Error(string propertyName, Exception innerException) : this(innerException)
    {
        Guard.NotNull(propertyName, nameof(propertyName));
        PropertyName = propertyName;
    }
}