using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using KFA.SupportAssistant.Globals.Models;

namespace KFA.SupportAssistant.UseCases.Users;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserChangePasswordCommand(string userId, string currentPassword, string newPassword, string? device) : Ardalis.SharedKernel.ICommand<Result>;

