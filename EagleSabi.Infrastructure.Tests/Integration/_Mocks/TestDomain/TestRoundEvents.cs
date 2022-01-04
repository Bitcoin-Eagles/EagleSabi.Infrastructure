using EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Models;

namespace EagleSabi.Infrastructure.Tests.Integration._Mocks.TestDomain
{
    public record RoundStarted(ulong MinInputSats) : IEvent;
    public record InputRegistered(string InputId, ulong Sats) : IEvent;
    public record InputUnregistered(string InputId) : IEvent;
    public record SigningStarted() : IEvent;
    public record RoundSucceeded(string TxId) : IEvent;
    public record RoundFailed(string Reason) : IEvent;
}