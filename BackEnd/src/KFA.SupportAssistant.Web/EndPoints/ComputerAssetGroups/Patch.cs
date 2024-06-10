using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetGroupRequest, ComputerAssetGroupRecord>
{
  private const string EndPointId = "ENP-176";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetGroupRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetGroupDTO, ComputerAssetGroup, PatchComputerAssetGroupRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Asset Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer asset group";
      s.Description = "Used to update part of an existing computer asset group. A valid existing computer asset group is required.";
      s.ResponseExamples[200] = new ComputerAssetGroupRecord(true, "Description", "1000", "Group Name", "Last Assigned Value", "", "Prefix", "Suffix", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetGroupRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupID))
    {
      AddError(request => request.GroupID, "The computer asset group of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetGroupDTO patchFunc(ComputerAssetGroupDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetGroupDTO, ComputerAssetGroup>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetGroupDTO, ComputerAssetGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer asset group to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetGroupRecord(obj.CanBeAssigned, obj.Description, obj.Id, obj.GroupName, obj.LastAssignedValue, obj.ParentGroupID, obj.Prefix, obj.Suffix, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
