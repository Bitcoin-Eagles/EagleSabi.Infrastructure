using System;
using System.Runtime.Serialization;

namespace EagleSabi.Infrastructure.Tests._Common.Exceptions;

public class TestException : Exception
{
    public TestException()
    {
    }

    public TestException(string? message) : base(message)
    {
    }

    public TestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}