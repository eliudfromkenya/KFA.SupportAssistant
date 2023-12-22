using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

/// <summary>
/// Update an existing user right.
/// </summary>
/// <remarks>
/// Update an existing user right by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateUserRightRequest, UpdateUserRightResponse>
{
  private const string EndPointId = "ENP-257";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateUserRightRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update User Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full User Right";
      s.Description = "This endpoint is used to update  user right, making a full replacement of user right with a specifed valuse. A valid user right is required.";
      s.ExampleRequest = new UpdateUserRightRequest { Description = "Description", Narration = "Narration", ObjectName = "Object Name", RightId = string.Empty, RoleId = string.Empty, UserId = string.Empty, UserRightId = "1000" };
      s.ResponseExamples[200] = new UpdateUserRightResponse(new UserRightRecord("Description", "Narration", "Object Name", string.Empty, string.Empty, string.Empty, "1000", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateUserRightRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.UserRightId))
    {
      AddError(request => request.UserRightId, "The user right id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<UserRightDTO, UserRight>(CreateEndPointUser.GetEndPointUser(User), request.UserRightId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the user right to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<UserRightDTO, UserRight>(CreateEndPointUser.GetEndPointUser(User), request.UserRightId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateUserRightResponse(new UserRightRecord(obj.Description, obj.Narration, obj.ObjectName, obj.RightId, obj.RoleId, obj.UserId, obj.Id, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
