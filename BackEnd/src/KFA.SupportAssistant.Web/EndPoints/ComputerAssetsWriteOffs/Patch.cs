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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetsWriteOffRequest, ComputerAssetsWriteOffRecord>
{
  private const string EndPointId = "ENP-1G6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetsWriteOffRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetsWriteOffDTO, ComputerAssetsWriteOff, PatchComputerAssetsWriteOffRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Assets Write Off End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer assets write off";
      s.Description = "Used to update part of an existing computer assets write off. A valid existing computer assets write off is required.";
      s.ResponseExamples[200] = new ComputerAssetsWriteOffRecord("", "Description", "Narration", "Reason For Write-Off", "1000", DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetsWriteOffRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.WriteOffID))
    {
      AddError(request => request.WriteOffID, "The computer assets write off of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetsWriteOffDTO patchFunc(ComputerAssetsWriteOffDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetsWriteOffDTO, ComputerAssetsWriteOff>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetsWriteOffDTO, ComputerAssetsWriteOff>(CreateEndPointUser.GetEndPointUser(User), request.WriteOffID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer assets write off to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetsWriteOffRecord(obj.AssetDetailID, obj.Description, obj.Narration, obj.ReasonForWriteOff, obj.Id, obj.WriteOffDate, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
