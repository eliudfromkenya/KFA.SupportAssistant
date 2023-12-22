using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

/// <summary>
/// List all system users by specified conditions
/// </summary>
/// <remarks>
/// List all system users - returns a SystemUserListResponse containing the system users.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, SystemUserListResponse>
{
  private const string EndPointId = "ENP-215";
  public const string Route = "/system_users";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get System Users List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of system users as specified";
      s.Description = "Returns all system users as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new SystemUserListResponse { SystemUsers = [new SystemUserRecord("Contact", "Email Address", DateTime.Now, true, DateTime.Now, "Name Of The User", "Narration", string.Empty, "1000", "Username", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<SystemUserDTO, SystemUser>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new SystemUserListResponse
      {
        SystemUsers = result.Value.Select(obj => new SystemUserRecord(obj.Contact, obj.EmailAddress, obj.ExpirationDate, obj.IsActive, obj.MaturityDate, obj.NameOfTheUser, obj.Narration, obj.RoleId, obj.Id, obj.Username, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
