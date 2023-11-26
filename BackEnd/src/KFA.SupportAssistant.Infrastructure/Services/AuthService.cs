using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Globals.Models;
using KFA.SupportAssistant.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KFA.SupportAssistant.Infrastructure.Services;

internal class AuthService(AppDbContext context, IIdGenerator idGenerator) : IAuthService
{
  private readonly AppDbContext _context = context;
  private readonly IIdGenerator _idGenerator = idGenerator;

  public async Task<LoginResult?> LoginAsync(string username, string password, string? device, CancellationToken cancellationToken)
  {
    using (_context)
    {
      var users = _context.SystemUsers
        .Where(c => c.Username!.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        // .Where(c => c.IsActive && c.MaturityDate >  DateTime.UtcNow && c.ExpirationDate < DateTime.UtcNow)
        .Select(b => new { b.Id, b.ExpirationDate, b.IsActive, b.MaturityDate, b.NameOfTheUser, b.PasswordHash, b.PasswordSalt, b.RoleId, b.Username, })
        .ToArray();

      users = users.Where(user => VerifyUser(password, user.PasswordHash, user.PasswordSalt)).ToArray();
      if (users.Length == 0) throw new Exception("Invalid user login credentials\r\nPlease check username or password");

      users = users.Where(user => user.IsActive).ToArray();
      if (users.Length == 0) throw new Exception("User is inactive, please contact system administrator");

      users = users.Where(user => user.MaturityDate < DateTime.UtcNow).ToArray();
      if (users.Length == 0) throw new Exception($"Your time to use the system is not yet ready, please contact system administrator");

      users = users.Where(user => user.ExpirationDate > DateTime.UtcNow).ToArray();
      if (users.Length == 0) throw new Exception($"You are no longer allowed t nuse the system, please contact system administrator");

      var user = users.First();
      var allUserRights = _context
        .UserRights
        .Where(c => c.UserId == user.Id || c.RoleId == user.RoleId)
        .Select(v => new { v.RightId, v.CommandId }).ToArray();
      var userRights = allUserRights
        .Where(c => c.CommandId != null && c.CommandId?.Trim()?.Length > 0)
        .Select(c => $"C-{c.CommandId}").Concat(allUserRights
        .Where(c => c.RightId != null && c.RightId?.Trim()?.Length > 0)
        .Select(c => $"R-{c.RightId}")).ToList();

      userRights.Add($"U-{user.RoleId}");

      var dataDevice = _context.DataDevices.FirstOrDefault(c => c.DeviceCode == device || c.DeviceNumber == device);

      var id = new IdGenerator().GetNextId<UserLogin>();
      var login = new UserLogin
      {
        DeviceId = dataDevice?.Id,
        FromDate = DateTime.UtcNow,
        Narration = "Web Api Login",
        UserId = user.Id,
        Id = id,
        ___DateInserted___ = DateTime.UtcNow.FromDateTime(),
        ___DateUpdated___ = DateTime.UtcNow.FromDateTime(),
        ___ModificationStatus___ = 1,
        UptoDate = DateTime.UtcNow
      };
      await _context.AddAsync(login, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
      return new LoginResult
      {
        DeviceId = dataDevice?.Id,
        ExpiryDate = DateTime.Now,
        LoginId = login.Id,
        Prefix = dataDevice?.DeviceNumber,
        UserId = user.Id,
        UserRole = user.RoleId,
        UserRights = userRights.ToArray()
      };
    }
  }

  private static bool VerifyUser(string password, byte[]? passwordHash, byte[]? passwordSalt)
  {
    try
    {
      if (passwordHash != null && passwordSalt != null)
      {
        using var hmac = new global::System.Security.Cryptography.HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(global::System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
          if (computedHash[i] != passwordHash[i]) return false;
        return true;
      }
    }
    catch
    {
    }
    return false;
  }

  public async Task<SystemUserDTO> RegisterUserAsync(SystemUserDTO mUser, string password, CancellationToken cancellationToken)
  {
    using (_context)
    {
      var usr = (SystemUser)mUser;
      byte[] passwordHash, passwordSalt = Array.Empty<byte>();
      passwordHash = password.CreatePasswordHash(out passwordSalt);

      SystemUser user = usr with { PasswordHash = passwordHash, Username = usr.Username?.ToLower(), PasswordSalt = passwordSalt, RoleId = "USR006" };

      await _context.SystemUsers.AddAsync(user, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
      return user;
    }
  }

  public async Task<bool> ChangeUserRoleAsync(string userId, string newRoleId, string oldRoleId, CancellationToken cancellationToken)
  {
    using (_context)
    {
      var user = _context.SystemUsers
       .Where(b => (b.Username == userId || b.Id == userId) && b.RoleId == oldRoleId)
       .FirstOrDefault();
      if (user == null) return false;

      user = user with { RoleId = newRoleId };
      _context.SystemUsers.Update(user);
      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }
  }

  public async Task<bool> ClearUserRightsAsync(string userId, CancellationToken cancellationToken, params string[] rightsToClear)
  {
    using (_context)
    {
      var rights = _context.UserRights
       .Where(b => b.UserId == userId || b.User!.Username == userId)
       .ToArray();

      if (rightsToClear?.Length > 0)
        rights = rights.Where(v => rightsToClear.Contains(v.Id)).ToArray();

      _context.UserRights.RemoveRange(rights);
      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }
  }

  public async Task<bool> AddUserRightsAsync(string userId, CancellationToken cancellationToken, params string[] rightsToAdd)
  {
    using (_context)
    {
      var user = _context.SystemUsers
      .Where(b => b.Username == userId || b.Id == userId)
      .FirstOrDefault() ?? throw new InvalidOperationException("Can't find the user");
      if (rightsToAdd?.Length < 1) throw new InvalidOperationException("Rights to add are required please");

      var rights = await _context
        .SystemRights
        .Where(n => rightsToAdd!.Contains(n.RightName) && rightsToAdd!.Contains(n.Id))
        .ToArrayAsync(cancellationToken);

      var userRights = rights.Select(v => new UserRight
      {
        Description = v.RightName,
        UserId = user.Id,
        RightId = v.Id,
        Narration = "Changed via web Api",
        ___ModificationStatus___ = 1,
        ___DateInserted___ = DateTime.UtcNow.FromDateTime(),
        ___DateUpdated___ = DateTime.UtcNow.FromDateTime()
      }).Select(n =>
      {
        n.Id = _idGenerator.GetNextId<UserRight>();
        return n;
      }).ToArray();

      await _context.UserRights.AddRangeAsync(userRights, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }
  }

  public async Task<bool> ChangePasswordAsync(string userIdOrUsername, string oldPassword, string newPassword, CancellationToken cancellationToken)
  {
    using (_context)
    {
      if (string.IsNullOrWhiteSpace(newPassword?.Trim()) || newPassword?.Trim()?.Length < 4)
        throw new Exception("New password must be atleast 4 characters");

      using var db = _context;
      var user = _context.SystemUsers
      .Where(b => b.Username == userIdOrUsername || b.Id == userIdOrUsername)
      .FirstOrDefault() ?? throw new Exception("Can't find the user to change the password");
      byte[] passwordHash, passwordSalt = [];
      passwordHash = newPassword!.CreatePasswordHash(out passwordSalt);

      if (!VerifyUser(oldPassword, user.PasswordHash, user.PasswordSalt))
        throw new Exception("Your current(old) password is not valid");

      var userX = db.SystemUsers
          .FirstOrDefault(c => c.Id == user.Id);

      user = user with { IsActive = true, PasswordHash = passwordHash, PasswordSalt = passwordSalt };

      _context.SystemUsers.Update(user);
      await _context.SaveChangesAsync(cancellationToken);
      return true;
    }
  }

  public async Task<DataDeviceDTO> RegisterDeviceAsync(DataDeviceDTO dataDevice, CancellationToken cancellationToken)
  {
    using (_context)
    {
      using var db = _context;
      DataDevice device = (DataDevice)dataDevice;
      if (string.IsNullOrWhiteSpace(device.Id))
        device.Id = _idGenerator.GetNextId<DataDevice>();

      await db.DataDevices.AddAsync(device, cancellationToken);
      await db.SaveChangesAsync(cancellationToken);
      return device;
    }
  }
}
