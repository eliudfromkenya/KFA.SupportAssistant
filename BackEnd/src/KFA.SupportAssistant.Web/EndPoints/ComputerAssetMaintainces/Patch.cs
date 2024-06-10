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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetMaintainceRequest, ComputerAssetMaintainceRecord>
{
  private const string EndPointId = "ENP-186";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetMaintainceRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetMaintainceDTO, ComputerAssetMaintaince, PatchComputerAssetMaintainceRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Asset Maintaince End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer asset maintaince";
      s.Description = "Used to update part of an existing computer asset maintaince. A valid existing computer asset maintaince is required.";
      s.ResponseExamples[200] = new ComputerAssetMaintainceRecord("", "", "Asset State", "Description", "Diagnosis", "Done By", "1000", "Narration", true, "What Was Done", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetMaintainceRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.MaintainceID))
    {
      AddError(request => request.MaintainceID, "The computer asset maintaince of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetMaintainceDTO patchFunc(ComputerAssetMaintainceDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetMaintainceDTO, ComputerAssetMaintaince>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetMaintainceDTO, ComputerAssetMaintaince>(CreateEndPointUser.GetEndPointUser(User), request.MaintainceID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer asset maintaince to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetMaintainceRecord(obj.AssetDispatchID, obj.AssetRecieveID, obj.AssetState, obj.Description, obj.Diagnosis, obj.DoneBy, obj.Id, obj.Narration, obj.TreatAsExpense, obj.WhatWasDone, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
