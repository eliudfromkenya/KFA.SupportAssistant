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
public class ChangePassword(IMediator mediator) : Endpoint<ChangePasswordRequest>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post(ChangePasswordRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Create a new Contributor.";
      //s.Description = "Create a new Contributor. A valid name is required.";
      s.ExampleRequest = new ChangePasswordRequest { Username = "Username", CurrentPassword = "password", NewPassword = "NewPassword" };
    });
  }

  public override async Task HandleAsync(
    ChangePasswordRequest request,
    CancellationToken cancellationToken)
  {
    var command = new UserChangePasswordCommand(request.Username!, request.CurrentPassword!, request.NewPassword!, request.Device);
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }
    if (result.IsSuccess)
    {
      await SendNoContentAsync(cancellationToken);
    }
    else await SendErrorsAsync(statusCode: 500, cancellationToken);
  }
}
