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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

/// <summary>
/// Update an existing computer asset group.
/// </summary>
/// <remarks>
/// Update an existing computer asset group by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetGroupRequest, UpdateComputerAssetGroupResponse>
{
  private const string EndPointId = "ENP-177";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetGroupRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Asset Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Asset Group";
      s.Description = "This endpoint is used to update  computer asset group, making a full replacement of computer asset group with a specifed valuse. A valid computer asset group is required.";
      s.ExampleRequest = new UpdateComputerAssetGroupRequest { CanBeAssigned = true, Description = "Description", GroupID = "1000", GroupName = "Group Name", LastAssignedValue = "Last Assigned Value", ParentGroupID = "", Prefix = "Prefix", Suffix = "Suffix" };
      s.ResponseExamples[200] = new UpdateComputerAssetGroupResponse(new ComputerAssetGroupRecord(true, "Description", "1000", "Group Name", "Last Assigned Value", "", "Prefix", "Suffix", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetGroupRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupID))
    {
      AddError(request => request.GroupID, "The group id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetGroupDTO, ComputerAssetGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer asset group to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetGroupDTO, ComputerAssetGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetGroupResponse(new ComputerAssetGroupRecord(obj.CanBeAssigned, obj.Description, obj.Id, obj.GroupName, obj.LastAssignedValue, obj.ParentGroupID, obj.Prefix, obj.Suffix, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
