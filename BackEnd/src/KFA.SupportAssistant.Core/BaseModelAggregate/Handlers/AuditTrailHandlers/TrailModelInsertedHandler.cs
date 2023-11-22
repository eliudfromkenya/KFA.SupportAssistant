using KFA.SupportAssistant.Core.BaseModelAggregate.Events;
using KFA.SupportAssistant.Globals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KFA.SupportAssistant.Core.ContributorAggregate.Handlers;

/// <summary>
/// NOTE: Internal because model  is also marked as internal.
/// </summary>
internal class TrailModelInsertedHandler<T>(ILogger<TrailModelInsertedHandler<T>> _logger) : INotificationHandler<ModelInsertedEvent<T>> where T : BaseModel
{
  public async Task Handle(ModelInsertedEvent<T> domainEvent, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Handling models inserted event for {contributorId}", string.Join(", ", domainEvent.Models.Select(c => c.Id)));

    await Task.Run(() => Functions.RunOnBackground(async () =>
    {
      try
      {
        // TODO: procedure to save insert audit trail to be added here
        await Task.Delay(1);
      }
      catch (Exception)
      {
      }
    }), cancellationToken);
  }
}
