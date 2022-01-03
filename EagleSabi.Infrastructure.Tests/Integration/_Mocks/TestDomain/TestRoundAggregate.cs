using System.Collections.Immutable;
using EagleSabi.Common.Abstractions.EventSourcing.Models;

namespace EagleSabi.Infrastructure.Tests.Integration._Mocks.TestDomain
{
    internal class TestRoundAggregate : IAggregate
    {
        public TestRoundState State { get; private set; } =
            new(0, TestRoundStatusEnum.New, ImmutableList<TestRoundInputState>.Empty, null, null);

        IState IAggregate.State => State;

        public void Apply(RoundStarted @event)
        {
            State = State with
            {
                Status = TestRoundStatusEnum.Started,
                MinInputSats = @event.MinInputSats,
            };
        }

        public void Apply(InputRegistered @event)
        {
            State = State with
            {
                Inputs = State.Inputs.Add(new(@event.InputId, @event.Sats))
            };
        }

        public void Apply(InputUnregistered @event)
        {
            State = State with
            {
                Inputs = State.Inputs.RemoveAll(a => a.InputId == @event.InputId),
            };
        }

        public void Apply(SigningStarted @event)
        {
            State = State with
            {
                Status = TestRoundStatusEnum.Signing,
            };
        }

        public void Apply(RoundSucceeded @event)
        {
            State = State with
            {
                Status = TestRoundStatusEnum.Succeeded,
                TxId = @event.TxId,
            };
        }

        public void Apply(RoundFailed @event)
        {
            State = State with
            {
                Status = TestRoundStatusEnum.Failed,
                FailureReason = @event.Reason,
            };
        }

        void IAggregate.Apply(IEvent ev)
        {
            ApplyDynamic(ev);
        }

        public void ApplyDynamic(dynamic ev)
        {
            Apply(ev);
        }
    }
}