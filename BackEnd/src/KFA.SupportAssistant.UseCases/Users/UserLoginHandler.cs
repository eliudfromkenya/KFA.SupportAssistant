using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Globals.Models;

namespace KFA.SupportAssistant.UseCases.Contributors.Create;

public class UserLoginHandler : ICommandHandler<UserLoginCommand, Result<LoginResult>>
{
  private readonly IAuthService _authService;

  public UserLoginHandler(IAuthService authService)
  {
    _authService = authService;
  }

  public async Task<Result<LoginResult>> Handle(UserLoginCommand request,
    CancellationToken cancellationToken)
  {
    LoginResult? result = await _authService.LoginAsync(request.username, request.password, request.device, cancellationToken);

    if (result == null)
    {
      return Result.Unauthorized();
    }
    return result;
  }
}
