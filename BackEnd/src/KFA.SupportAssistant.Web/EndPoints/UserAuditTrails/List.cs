using Ardalis.Result;
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

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

/// <summary>
/// List all user audit trails by specified conditions
/// </summary>
/// <remarks>
/// List all user audit trails - returns a UserAuditTrailListResponse containing the user audit trails.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, UserAuditTrailListResponse>
{
  private const string EndPointId = "ENP-235";
  public const string Route = "/user_audit_trails";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get User Audit Trails List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of user audit trails as specified";
      s.Description = "Returns all user audit trails as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new UserAuditTrailListResponse { UserAuditTrails = [new UserAuditTrailRecord(DateTime.Now, 0, "1000", "Category", "0", "Data", "Description", "0", "Narration", "Old Values", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<UserAuditTrailDTO, UserAuditTrail>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<UserAuditTrailDTO>>.Success(ans.Select(v => (UserAuditTrailDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new UserAuditTrailListResponse
      {
        UserAuditTrails = result.Value.Select(obj => new UserAuditTrailRecord(obj.ActivityDate, obj.ActivityEnumNumber, obj.Id, obj.Category, obj.CommandId, obj.Data, obj.Description, obj.LoginId, obj.Narration, obj.OldValues, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
