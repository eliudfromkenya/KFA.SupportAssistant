using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Web.UserEndPoints;

namespace KFA.SupportAssistant.UseCases.Users;

public class UserRegisterDeviceHandler(IAuthService userService) : ICommandHandler<UserRegisterDeviceCommand, Result<DataDeviceDTO>>
{
  public async Task<Result<DataDeviceDTO>> Handle(UserRegisterDeviceCommand request,
    CancellationToken cancellationToken)
  {
    return await userService.RegisterDeviceAsync(request.device, cancellationToken);
  }
}

