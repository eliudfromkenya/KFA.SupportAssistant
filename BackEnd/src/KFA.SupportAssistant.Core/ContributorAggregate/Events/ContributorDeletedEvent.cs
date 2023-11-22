using Ardalis.SharedKernel;

namespace KFA.SupportAssistant.Core.ContributorAggregate.Events;

/// <summary>
/// A domain event that is dispatched whenever a gontributor is deleted.
/// The DeleteContributorService is used to dispatch this event.
/// </summary>
internal sealed class ContributorDeletedEvent(string contributorId) : DomainEventBase
{
  public string ContributorId { get; init; } = contributorId;
}
