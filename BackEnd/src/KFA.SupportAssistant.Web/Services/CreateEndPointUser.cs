using KFA.SupportAssistant.Core;

namespace KFA.SupportAssistant.Web.Services;

public static class CreateEndPointUser
{
  public static EndPointUser GetEndPointUser(System.Security.Claims.ClaimsPrincipal user) 
  {
    var loginId = user.Claims.FirstOrDefault(c => c.Type == "LoginId")?.Value;
    var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
    var roleId = user.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
    var rights = user.Claims.Where(c => c.Type == "permissions").Select(c => c.Value).ToArray();
    return  new EndPointUser { LoginId = loginId, Rights = rights, RoleId=roleId, UserId=userId };
  }
}
