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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetsStatusRequest, ComputerAssetsStatusRecord>
{
  private const string EndPointId = "ENP-1F6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetsStatusRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetsStatusDTO, ComputerAssetsStatus, PatchComputerAssetsStatusRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Assets Status End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer assets status";
      s.Description = "Used to update part of an existing computer assets status. A valid existing computer assets status is required.";
      s.ResponseExamples[200] = new ComputerAssetsStatusRecord("", "1000", DateTime.Now, "Description", "Done By", "Status", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetsStatusRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AssetsStatusID))
    {
      AddError(request => request.AssetsStatusID, "The computer assets status of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetsStatusDTO patchFunc(ComputerAssetsStatusDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetsStatusDTO, ComputerAssetsStatus>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetsStatusDTO, ComputerAssetsStatus>(CreateEndPointUser.GetEndPointUser(User), request.AssetsStatusID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer assets status to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetsStatusRecord(obj.AssetDetailID, obj.Id, obj.Date, obj.Description, obj.DoneBy, obj.Status, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
