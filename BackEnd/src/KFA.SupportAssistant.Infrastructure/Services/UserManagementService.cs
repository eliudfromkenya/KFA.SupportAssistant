using System.Reflection.Emit;
using Ardalis.Result;
using KFA.SupportAssistant;
using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

internal class UserManagementService(AppDbContext context, IIdGenerator idGenerator) : IUserManagementService
{
  public async Task<Result<UserRightDTO[]>> AddUserRightsAsync(string userId, string[] rightsToAdd, string[] commandIds, CancellationToken cancellationToken)
  {
    using (context)
    {
      var user = context.SystemUsers
      .Where(b => b.Username == userId || b.Id == userId)
      .FirstOrDefault() ?? throw new InvalidOperationException("Can't find the user");
      if (rightsToAdd?.Length < 1) throw new InvalidOperationException("Rights to add are required please");

      var commands = await context
        .CommandDetails
        .Where(n => commandIds!.Contains(n.CommandName) || commandIds!.Contains(n.CommandText) || commandIds!.Contains(n.Id))
        .ToArrayAsync(cancellationToken);
      var rights = await context
       .SystemRights
       .Where(n => rightsToAdd!.Contains(n.RightName) || rightsToAdd!.Contains(n.Id))
       .ToArrayAsync(cancellationToken);

      if(rights?.Length < 1 && commands?.Length < 0)
        return Result.Error("Can't find the specified right or command to add");

      var userRights = rights!.Select(v => new UserRight
      {
        Description = v.RightName,
        UserId = user.Id,
        RightId = v.Id,
        ObjectName = v.RightName,
        Narration = "Changed via web Api",
        // ___ModificationStatus___ = 1,
        ___DateInserted___ = DateTime.UtcNow.FromDateTime(),
        ___DateUpdated___ = DateTime.UtcNow.FromDateTime()
      }).ToArray();

      var userCommands = commands!.Select(v => new UserRight
      {
        Description = v.CommandName,
        ObjectName = v.CommandName,
        UserId = user.Id,
        CommandId = v.Id,
        UserActivities = UserActivities.None,
        Narration = "Changed via web Api",
        // ___ModificationStatus___ = 1,
        ___DateInserted___ = DateTime.UtcNow.FromDateTime(),
        ___DateUpdated___ = DateTime.UtcNow.FromDateTime()
      }).ToArray();

      var cmdIds = userCommands.Select(c => c.CommandId).ToArray();
      var rghtIds = userRights.Select(c => c.RightId).ToArray();
      if(rghtIds.Length > 0)
      {
        var rghts = context.UserRights
          .Where(c => c.UserId == user.Id && rghtIds.Contains(c.RightId))
          .Select(c => c.RightId)
          .ToArray();
        if (rghts.Length != 0)
          userRights = userRights
            .Where(c => !rghts.Contains(c.RightId)).ToArray();
      }
      if (cmdIds.Length > 0)
      {
        var cmds = context.UserRights
          .Where(c => c.UserId == user.Id && cmdIds.Contains(c.CommandId))
          .Select(c => c.CommandId)
          .ToArray();
        if (cmds.Length != 0)
          userCommands = userCommands
            .Where(c => !cmds.Contains(c.CommandId)).ToArray();
      }

      userRights = userCommands.Union(userRights).Select(n =>
      {
        n.Id = idGenerator.GetNextId<UserRight>();
        return n;
      }).ToArray();

      if (userRights.Length != 0)
      {
        await context.UserRights.AddRangeAsync(userRights, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return userRights?.Select(x => (UserRightDTO)x)?.ToArray() ?? [];
      }
      else return Result.Error("Rights have already been added");
    }
  }
 public async Task<Result> ChangeUserRoleAsync(string userId, string newRoleId, string? device, CancellationToken cancellationToken)
  {
    using (context)
    {
      var user = context.SystemUsers
       .Where(b => b.Username == userId || b.Id == userId)
       .AsNoTracking()
       .FirstOrDefault();
      if (user == null)
        return Result.Error("Unable to find the user to change the password");

      user = user! with { RoleId = newRoleId };
      context.SystemUsers.Update(user);
      await context.SaveChangesAsync(cancellationToken);
      return Result.Success();
    }
  }

  public async Task<Result<string[]>> ClearUserRightsAsync(string userId, string? device, CancellationToken cancellationToken, params string[] rightsToClear)
  {
    using (context)
    {
      var rights = context.UserRights
       .Where(b => b.UserId == userId || b.User!.Username == userId)
       .ToArray();

      if (rightsToClear?.Length > 0)
        rights = rights.Where(v => rightsToClear.Contains(v.Id)).ToArray();

      context.UserRights.RemoveRange(rights);
      await context.SaveChangesAsync(cancellationToken);
      return Result.Success(rights.Select(c => c.Id!).ToArray());
    }
  }
}
