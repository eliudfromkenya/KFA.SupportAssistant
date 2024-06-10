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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

/// <summary>
/// Update an existing computer asset maintaince.
/// </summary>
/// <remarks>
/// Update an existing computer asset maintaince by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetMaintainceRequest, UpdateComputerAssetMaintainceResponse>
{
  private const string EndPointId = "ENP-187";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetMaintainceRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Asset Maintaince End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Asset Maintaince";
      s.Description = "This endpoint is used to update  computer asset maintaince, making a full replacement of computer asset maintaince with a specifed valuse. A valid computer asset maintaince is required.";
      s.ExampleRequest = new UpdateComputerAssetMaintainceRequest { AssetDispatchID = "", AssetRecieveID = "", AssetState = "Asset State", Description = "Description", Diagnosis = "Diagnosis", DoneBy = "Done By", MaintainceID = "1000", Narration = "Narration", TreatAsExpense = true, WhatWasDone = "What Was Done" };
      s.ResponseExamples[200] = new UpdateComputerAssetMaintainceResponse(new ComputerAssetMaintainceRecord("", "", "Asset State", "Description", "Diagnosis", "Done By", "1000", "Narration", true, "What Was Done", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetMaintainceRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.MaintainceID))
    {
      AddError(request => request.MaintainceID, "The maintaince id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetMaintainceDTO, ComputerAssetMaintaince>(CreateEndPointUser.GetEndPointUser(User), request.MaintainceID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer asset maintaince to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetMaintainceDTO, ComputerAssetMaintaince>(CreateEndPointUser.GetEndPointUser(User), request.MaintainceID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetMaintainceResponse(new ComputerAssetMaintainceRecord(obj.AssetDispatchID, obj.AssetRecieveID, obj.AssetState, obj.Description, obj.Diagnosis, obj.DoneBy, obj.Id, obj.Narration, obj.TreatAsExpense, obj.WhatWasDone, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
