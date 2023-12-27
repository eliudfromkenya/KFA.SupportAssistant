using System.Security.Claims;
using FastEndpoints.Security;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
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
public class Login : Endpoint<LoginRequest, LoginResponse>
{
  private readonly IMediator _mediator;

  //private readonly Microsoft.Extensions.Configuration.ConfigurationManager _manager;

  // private readonly WebApplicationBuilder _builder;

  public Login(IMediator mediator)
  {
    _mediator = mediator;
    //_manager = manager;
    // _builder = builder;
  }

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(LoginRequest.Route));
    AllowAnonymous();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Create a new Contributor.";
      //s.Description = "Create a new Contributor. A valid name is required.";
      s.ExampleRequest = new LoginRequest { Username = "Username", Password = "password" };
    });
  }

  public override async Task HandleAsync(
    LoginRequest request,
    CancellationToken cancellationToken)
  {
    var tokenSignature = Config.GetValue<string>("Auth:TokenSigningKey");

    var command = new UserLoginCommand(request.Username!, request.Password!, request.Device);
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      var value = result.Value;
      var jwtToken = JWTBearer.CreateToken(
          signingKey: tokenSignature!,
          expireAt: DateTime.UtcNow.AddDays(30),
          permissions: value.UserRights!,
          claims: new Claim[]
          {
            new ("UserId", value.UserId!) ,
            new ("LoginId", value.LoginId!) ,
            new ("RoleId", value.UserRole!)
          });

      await SendAsync(new LoginResponse(value.LoginId, jwtToken, value.UserId, value.UserRole, DateTime.Now, value.UserRights, value.User as SystemUserDTO), cancellation: cancellationToken);
    }
    else
    {
      AddError("The supplied credentials are invalid!");
    }
  }
}
