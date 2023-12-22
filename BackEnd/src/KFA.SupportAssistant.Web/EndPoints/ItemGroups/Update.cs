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

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

/// <summary>
/// Update an existing item group.
/// </summary>
/// <remarks>
/// Update an existing item group by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateItemGroupRequest, UpdateItemGroupResponse>
{
  private const string EndPointId = "ENP-1H7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateItemGroupRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Item Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Item Group";
      s.Description = "This endpoint is used to update  item group, making a full replacement of item group with a specifed valuse. A valid item group is required.";
      s.ExampleRequest = new UpdateItemGroupRequest { GroupId = "1000", Name = "Name", ParentGroupId = "Parent Group Id" };
      s.ResponseExamples[200] = new UpdateItemGroupResponse(new ItemGroupRecord("1000", "Name", "Parent Group Id", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateItemGroupRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupId))
    {
      AddError(request => request.GroupId, "The group id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ItemGroupDTO, ItemGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the item group to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ItemGroupDTO, ItemGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateItemGroupResponse(new ItemGroupRecord(obj.Id, obj.Name, obj.ParentGroupId, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
