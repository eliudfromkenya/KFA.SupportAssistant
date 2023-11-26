using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.Services;

namespace KFA.SupportAssistant.UseCases.Users;

public class UserChangePasswordHandler(IAuthService userService) : ICommandHandler<UserChangePasswordCommand, Result>
{
  public async Task<Result> Handle(UserChangePasswordCommand request,
    CancellationToken cancellationToken)
  {
     return await userService.ChangePasswordAsync(request.userId, request.currentPassword, request.newPassword, request.device,cancellationToken);
  }
}
