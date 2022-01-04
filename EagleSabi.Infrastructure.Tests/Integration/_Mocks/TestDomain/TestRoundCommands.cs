using System;
using EagleSabi.Common.Abstractions.EventSourcing.Models;

namespace EagleSabi.Infrastructure.Tests.Integration._Mocks.TestDomain
{
    public record StartRound(ulong MinInputSats, Guid IdempotenceId) : ICommand;
    public record RegisterInput(string InputId, ulong Sats, Guid IdempotenceId) : ICommand;
    public record UnregisterInput(string InputId, Guid IdempotenceId) : ICommand;
    public record StartSigning(Guid IdempotenceId) : ICommand;
    public record SetSucceeded(string TxId, Guid IdempotenceId) : ICommand;
    public record SetFailed(string Reason, Guid IdempotenceId) : ICommand;
}