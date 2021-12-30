using System.Collections.Immutable;
using EagleSabi.Common.Abstractions.EventSourcing.Models;

namespace EagleSabi.Infrastructure.Tests.Integration.Mocks.TestDomain
{
    public record TestRoundState(
        ulong MinInputSats,
        TestRoundStatusEnum Status,
        ImmutableList<TestRoundInputState> Inputs,
        string? TxId,
        string? FailureReason) : IState;

    public record TestRoundInputState(string InputId, ulong Sats);
}