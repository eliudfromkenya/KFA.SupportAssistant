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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetDetailRequest, ComputerAssetDetailRecord>
{
  private const string EndPointId = "ENP-166";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetDetailRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetDetailDTO, ComputerAssetDetail, PatchComputerAssetDetailRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Asset Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer asset detail";
      s.Description = "Used to update part of an existing computer asset detail. A valid existing computer asset detail is required.";
      s.ResponseExamples[200] = new ComputerAssetDetailRecord("1000", "Asset Name", "Description", "Group ID", "Serial Number", "State", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetDetailRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AssetID))
    {
      AddError(request => request.AssetID, "The computer asset detail of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetDetailDTO patchFunc(ComputerAssetDetailDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetDetailDTO, ComputerAssetDetail>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetDetailDTO, ComputerAssetDetail>(CreateEndPointUser.GetEndPointUser(User), request.AssetID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer asset detail to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetDetailRecord(obj.Id, obj.AssetName, obj.Description, obj.GroupID, obj.SerialNumber, obj.State, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
