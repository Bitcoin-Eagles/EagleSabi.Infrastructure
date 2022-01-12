﻿using EagleSabi.Infrastructure.Common.Records.EventSourcing;

namespace EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Modules;

public interface IEventRepository
{
    /// <summary>
    /// Atomically persistently appends ordered list of events for given aggregate.
    /// In case of duplicate of <seealso cref="WrappedEvent.SequenceId"/>
    /// <seealso cref="OptimisticConcurrencyException"/> is thrown indicating that
    /// command should be retried with freshly loaded events from <see cref="ListEventsAsync"/>.
    /// All appended events go into a list of undelivered events 👉 <see cref="ListUndeliveredEventsAsync(int?)"/>
    /// </summary>
    /// <param name="aggregateType">Type of an aggregate</param>
    /// <param name="aggregateId">Id of an aggregate</param>
    /// <param name="wrappedEvents">Ordered list of events to be persisted</param>
    /// <exception cref="OptimisticConcurrencyException">
    /// If there is concurrency conflict ; retry command
    /// </exception>
    /// <exception cref="TransientException">Transient infrastructure failure</exception>
    /// <exception cref="ArgumentException">Invalid input</exception>
    public Task AppendEventsAsync(
        string aggregateType,
        string aggregateId,
        IEnumerable<WrappedEvent> wrappedEvents);

    /// <summary>
    /// List strongly ordered events of given aggregate. This is the primary source of truth.
    /// Events are used to reconstruct aggregate state before processing command.
    /// </summary>
    /// <param name="aggregateType">Type of an aggregate</param>
    /// <param name="aggregateId">Id of an aggregate</param>
    /// <param name="afterSequenceId">Starts with event after given <seealso cref="WrappedEvent.SequenceId"/>
    /// and lists all following events</param>
    /// <param name="maxCount">Limits the number of returned events</param>
    /// <returns>Ordered list of events</returns>
    /// <exception cref="TransientException">Transient infrastracture failure</exception>
    public Task<IReadOnlyList<WrappedEvent>> ListEventsAsync(
        string aggregateType,
        string aggregateId,
        long afterSequenceId = 0,
        int? maxCount = null);

    /// <summary>
    /// Cumulatively marks events as delivered (e.g. to message bus) for given <paramref name="aggregateId"/>.
    /// To be used in pair with <see cref="ListUndeliveredEventsAsync(int?)"/> method.
    /// </summary>
    /// <param name="aggregateType">Type of aggregate</param>
    /// <param name="aggregateId">Id of aggregate</param>
    /// <param name="deliveredSequenceId">sequenceId of last delivered event of the aggregate</param>
    public Task MarkEventsAsDeliveredCumulativeAsync(
        string aggregateType,
        string aggregateId,
        long deliveredSequenceId);

    /// <summary>
    /// Lists undelivered pending events to be delivered (e.g. to message bus) for all aggregates.
    /// Event is considered undelivered if it has been appended
    /// by <see cref="AppendEventsAsync(string, string, IEnumerable{WrappedEvent})"/>
    /// and not yet marked as delivered by <see cref="MarkEventsAsDeliveredCumulativeAsync(string, string, long)"/>.
    /// </summary>
    /// <returns>list of <see cref="AggregateUndeliveredEvents"/></returns>
    public Task<IReadOnlyList<AggregateUndeliveredEvents>> ListUndeliveredEventsAsync(int? maxCount = null);

    /// <summary>
    /// Supplementary method for enumerating all ids for <paramref name="aggregateType"/>
    /// in this event repository. Order of ids is not defined can be any artificial.
    /// </summary>
    /// <param name="aggregateType">Type of an aggregate</param>
    /// <param name="afterAggregateId">Starts with id right after given id and lists all following ids
    /// in any artificial order</param>
    /// <param name="maxCount">Limits the number of returned events</param>
    /// <returns>Unordered list of ids</returns>
    /// <exception cref="TransientException">Transient infrastracture failure</exception>
    public Task<IReadOnlyList<string>> ListAggregateIdsAsync(
        string aggregateType,
        string? afterAggregateId = null,
        int? maxCount = null);
}