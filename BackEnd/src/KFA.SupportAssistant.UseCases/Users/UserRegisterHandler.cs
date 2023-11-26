using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Services;

namespace KFA.SupportAssistant.UseCases.Users;

public class UserRegisterHandler(IAuthService userService) : ICommandHandler<UserRegisterCommand, Result<(SystemUserDTO user, string? loginId, string?[]? rights)>>
{
  public async Task<Result<(SystemUserDTO user, string? loginId, string?[]? rights)>> Handle(UserRegisterCommand request,
    CancellationToken cancellationToken)
  {
    var user = await userService.RegisterUserAsync(request.user, request.password, request.device, cancellationToken);
    var login = await userService.LoginAsync(request.user.Username!, request.password, request.device, cancellationToken);
    return Result.Success((user,login?.LoginId, login?.UserRights));
  }
}


