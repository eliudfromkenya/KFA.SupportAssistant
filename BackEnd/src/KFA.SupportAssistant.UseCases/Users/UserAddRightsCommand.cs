using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using KFA.SupportAssistant.Core.DTOs;

namespace KFA.SupportAssistant.UseCases.Users;
public record UserAddRightsCommand(string userId, string[] commandIds, string[] rightIds) : Ardalis.SharedKernel.ICommand<Result<UserRightDTO[]>>;
