using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;

namespace KFA.SupportAssistant.UseCases.Users;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserClearRightsCommand(string userId, string[] userRightsIds, string? device) : Ardalis.SharedKernel.ICommand<Result<string[]>>;
