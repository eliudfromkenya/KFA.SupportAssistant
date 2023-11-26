using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using KFA.SupportAssistant.Core.DTOs;

namespace KFA.SupportAssistant.UseCases.Users;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserRegisterCommand(SystemUserDTO user, string password) : Ardalis.SharedKernel.ICommand<Result<(SystemUserDTO user, string loginId, string[] rights)>>;
