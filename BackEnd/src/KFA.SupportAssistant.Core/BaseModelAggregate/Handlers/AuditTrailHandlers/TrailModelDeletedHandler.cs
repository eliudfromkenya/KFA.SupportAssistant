using KFA.SupportAssistant.Core.BaseModelAggregate.Events;
using KFA.SupportAssistant.Globals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KFA.SupportAssistant.Core.ContributorAggregate.Handlers;

/// <summary>
/// NOTE: Internal because model  is also marked as internal.
/// </summary>
internal class TrailModelDeletedHandler<T>(ILogger<TrailModelDeletedHandler<T>> _logger) : INotificationHandler<ModelDeletedEvent<T>> where T : BaseModel
{
  public async Task Handle(ModelDeletedEvent<T> domainEvent, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Handling models deleted event for {contributorId}", string.Join(", ", domainEvent.Ids?.Select(c => c.Id) ?? []));

    await Task.Run(() => Functions.RunOnBackground(async () =>
    {
      try
      {
        // TODO: procedure to save delete audit trail to be added here
        await Task.Delay(1);
      }
      catch (Exception)
      {
      }
    }), cancellationToken);
  }
}
