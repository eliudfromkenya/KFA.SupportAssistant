using Ardalis.Result;
using KFA.SupportAssistant.Core.DTOs;
using MediatR;

namespace KFA.SupportAssistant.Web.UserEndPoints;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserRegisterDeviceCommand(DataDeviceDTO device) : Ardalis.SharedKernel.ICommand<Result<DataDeviceDTO>>;
