using System.Runtime.Serialization;

namespace EagleSabi.Infrastructure.Common.Exceptions;

/// <summary>
/// Infrastructure or optimistic conflict exception that will be fixed when retried.
/// </summary>
public class TransientException : Exception
{
    public TransientException()
    {
    }

    public TransientException(string? message) : base(message)
    {
    }

    public TransientException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TransientException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}