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

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchStaffGroupRequest, StaffGroupRecord>
{
  private const string EndPointId = "ENP-1V6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchStaffGroupRequest.Route));
    //RequestBinder(new PatchBinder<StaffGroupDTO, StaffGroup, PatchStaffGroupRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Staff Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a staff group";
      s.Description = "Used to update part of an existing staff group. A valid existing staff group is required.";
      s.ResponseExamples[200] = new StaffGroupRecord("Description", "1000", true, "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchStaffGroupRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupNumber))
    {
      AddError(request => request.GroupNumber, "The staff group of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    StaffGroupDTO patchFunc(StaffGroupDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<StaffGroupDTO, StaffGroup>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<StaffGroupDTO, StaffGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupNumber ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the staff group to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new StaffGroupRecord(obj.Description, obj.Id, obj.IsActive, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
