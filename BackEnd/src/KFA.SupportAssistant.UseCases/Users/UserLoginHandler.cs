using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Globals.Models;

namespace KFA.SupportAssistant.UseCases.Users;

public class UserLoginHandler(IAuthService authService) : ICommandHandler<UserLoginCommand, Result<LoginResult>>
{
  public async Task<Result<LoginResult>> Handle(UserLoginCommand request,
    CancellationToken cancellationToken)
  {
    var result = await authService.LoginAsync(request.username, request.password, request.device, cancellationToken);

    if (result == null)
    {
      return Result.Unauthorized();
    }
    return result;
  }
}
