using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Core.Services;
using MediatR;

namespace KFA.SupportAssistant.UseCases.Users;

public class UserRegisterHandler(IAuthService userService, IMediator mediator) : ICommandHandler<UserRegisterCommand, Result<(SystemUserDTO user, string? loginId, string?[]? rights)>>
{
  public async Task<Result<(SystemUserDTO user, string? loginId, string?[]? rights)>> Handle(UserRegisterCommand request,
    CancellationToken cancellationToken)
  {
    var user = await userService.RegisterUserAsync(request.user, request.password, request.device, cancellationToken);
    var command = new UserLoginCommand(request.user.Username!, request.password!, request.device);
    var result = await mediator.Send(command, cancellationToken);
    string? loginId =string.Empty;
    string[] userRights = [];
    if (result?.Value != null)
    {
      loginId = result.Value.LoginId!;
      userRights = result.Value.UserRights!;
    }
    (SystemUserDTO user, string? loginId, string?[]? rights) ans = (user, loginId, userRights);
    return Result.Success(ans);
  }
}


