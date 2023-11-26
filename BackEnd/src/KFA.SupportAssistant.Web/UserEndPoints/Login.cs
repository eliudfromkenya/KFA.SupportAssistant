using System.Security.Claims;
using FastEndpoints.Security;
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
  private readonly IConfiguration _config;

  //private readonly Microsoft.Extensions.Configuration.ConfigurationManager _manager;

  // private readonly WebApplicationBuilder _builder;

  public Login(IMediator mediator, IConfiguration config)
  {
    _mediator = mediator;
    _config = config;
    //_manager = manager;
    // _builder = builder;
  }

  public override void Configure()
  {
    Post(LoginRequest.Route);
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
    var tokenSignature = _config.GetValue<string>("Auth:TokenSigningKey");

    var command = new UserLoginCommand(request.Username!, request.Password!, request.Device);
    var result = await _mediator.Send(command, cancellationToken);

    if (result.IsSuccess)
    {
      var value = result.Value;
      var jwtToken = JWTBearer.CreateToken(
          signingKey: tokenSignature!,
          expireAt: DateTime.UtcNow.AddDays(1),
          permissions: value.UserRights!,
          claims: new Claim[]
          {
            new ("UserId", value.UserId!) ,
            new ("LoginId", value.LoginId!) ,
            new ("RoleId", value.UserRole!)
          });

      await SendAsync(new LoginResponse(value.LoginId, jwtToken, value.UserId, value.UserRole, DateTime.Now, value.UserRights), cancellation: cancellationToken);
    }
    else
    {
      AddError("The supplied credentials are invalid!");
    }
  }
}
