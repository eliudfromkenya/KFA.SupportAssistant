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

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

/// <summary>
/// Update an existing staff group.
/// </summary>
/// <remarks>
/// Update an existing staff group by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateStaffGroupRequest, UpdateStaffGroupResponse>
{
  private const string EndPointId = "ENP-1V7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateStaffGroupRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Staff Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Staff Group";
      s.Description = "This endpoint is used to update  staff group, making a full replacement of staff group with a specifed valuse. A valid staff group is required.";
      s.ExampleRequest = new UpdateStaffGroupRequest { Description = "Description", GroupNumber = "1000", IsActive = true, Narration = "Narration" };
      s.ResponseExamples[200] = new UpdateStaffGroupResponse(new StaffGroupRecord("Description", "1000", true, "Narration", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateStaffGroupRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupNumber))
    {
      AddError(request => request.GroupNumber, "The group number of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<StaffGroupDTO, StaffGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupNumber ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the staff group to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<StaffGroupDTO, StaffGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupNumber ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateStaffGroupResponse(new StaffGroupRecord(obj.Description, obj.Id, obj.IsActive, obj.Narration, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
