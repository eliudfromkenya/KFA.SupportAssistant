using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Users;
using MediatR;

namespace KFA.SupportAssistant.Web.UserEndPoints;

/// <summary>
/// Create a new Contributor
/// </summary>
/// <remarks>
/// Creates a new Contributor given a name.
/// </remarks>
public class AddRights(IMediator mediator, IConfiguration config) : Endpoint<AddRightsRequest, AddRightsResponse[]>
{
  private readonly IMediator _mediator = mediator;
  private readonly IConfiguration _config = config;

  public override void Configure()
  {
    Post(AddRightsRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Create a new Contributor.";
      //s.Description = "Create a new Contributor. A valid name is required.";
      s.ExampleRequest = new AddRightsRequest { UserId = "Username", Rights = ["Right Id"], Commands = ["Command Id"] };
    });
  }

  public override async Task HandleAsync(
    AddRightsRequest request,
    CancellationToken cancellationToken)
  {
    var command = new UserAddRightsCommand(request.UserId!, request.Commands!, request.Rights!);
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      await SendAsync(result.Value.Select(v => new AddRightsResponse(v.Id, v.UserId, v.CommandId, v.ObjectName, v.RightId)).ToArray(), cancellation: cancellationToken);
    }
    else await SendErrorsAsync(statusCode: 500, cancellationToken);
  }
}
