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

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchItemGroupRequest, ItemGroupRecord>
{
  private const string EndPointId = "ENP-1H6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchItemGroupRequest.Route));
    //RequestBinder(new PatchBinder<ItemGroupDTO, ItemGroup, PatchItemGroupRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Item Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a item group";
      s.Description = "Used to update part of an existing item group. A valid existing item group is required.";
      s.ResponseExamples[200] = new ItemGroupRecord("1000", "Name", "Parent Group Id", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchItemGroupRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupId))
    {
      AddError(request => request.GroupId, "The item group of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ItemGroupDTO patchFunc(ItemGroupDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ItemGroupDTO, ItemGroup>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ItemGroupDTO, ItemGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the item group to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ItemGroupRecord(obj.Id, obj.Name, obj.ParentGroupId, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
