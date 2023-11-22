using KFA.SupportAssistant.Core.BaseModelAggregate.Events;
using KFA.SupportAssistant.Globals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KFA.SupportAssistant.Core.ContributorAggregate.Handlers;

/// <summary>
/// NOTE: Internal because model  is also marked as internal.
/// </summary>
internal class TrailModelUpdatedHandler<T>(ILogger<TrailModelUpdatedHandler<T>> _logger) : INotificationHandler<ModelUpdatedEvent<T>> where T : BaseModel
{
  public async Task Handle(ModelUpdatedEvent<T> domainEvent, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Handling models Updated event for {contributorId}", string.Join(", ", domainEvent.Id));

    await Task.Run(() => Functions.RunOnBackground(async () =>
    {
      try
      {
        // TODO: procedure to save update audit trail to be added here
        await Task.Delay(1);
      }
      catch (Exception)
      {
      }
    }), cancellationToken);
  }
}
