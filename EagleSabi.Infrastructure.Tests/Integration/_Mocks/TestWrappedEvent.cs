using System;
using EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Models;
using EagleSabi.Infrastructure.Common.Records.EventSourcing;

namespace EagleSabi.Infrastructure.Tests.Integration._Mocks
{
    public record TestWrappedEvent(long SequenceId, string Value = "", IEvent? DomainEvent = null, Guid SourceId = default) : WrappedEvent("", "", SequenceId, DomainEvent!, SourceId);
}