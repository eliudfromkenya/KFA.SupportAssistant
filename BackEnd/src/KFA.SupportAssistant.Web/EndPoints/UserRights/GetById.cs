using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

/// <summary>
/// Get a user right by user right id.
/// </summary>
/// <remarks>
/// Takes user right id and returns a matching user right record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetUserRightByIdRequest, UserRightRecord>
{
  private const string EndPointId = "ENP-254";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetUserRightByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get User Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets user right by specified user right id";
      s.Description = "This endpoint is used to retrieve user right with the provided user right id";
      s.ExampleRequest = new GetUserRightByIdRequest { UserRightId = "user right id to retrieve" };
      s.ResponseExamples[200] = new UserRightRecord("Description", "Narration", "Object Name", string.Empty, string.Empty, string.Empty, "1000", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetUserRightByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.UserRightId))
    {
      AddError(request => request.UserRightId, "The user right id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<UserRightDTO, UserRight>(CreateEndPointUser.GetEndPointUser(User), request.UserRightId ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UserRightRecord(obj.Description, obj.Narration, obj.ObjectName, obj.RightId, obj.RoleId, obj.UserId, obj.Id, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
