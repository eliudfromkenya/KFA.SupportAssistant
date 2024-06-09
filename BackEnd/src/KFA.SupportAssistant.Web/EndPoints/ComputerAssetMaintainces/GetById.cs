
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

/// <summary>
/// Get a computer asset maintaince by maintaince id.
/// </summary>
/// <remarks>
/// Takes maintaince id and returns a matching computer asset maintaince record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetMaintainceByIdRequest, ComputerAssetMaintainceRecord>
{
  private const string EndPointId = "ENP-184";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetMaintainceByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Asset Maintaince End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer asset maintaince by specified maintaince id";
      s.Description = "This endpoint is used to retrieve computer asset maintaince with the provided maintaince id";
      s.ExampleRequest = new GetComputerAssetMaintainceByIdRequest { MaintainceID = "maintaince id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetMaintainceRecord("", "", "Asset State", "Description", "Diagnosis", "Done By", "1000", "Narration", true, "What Was Done", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetMaintainceByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.MaintainceID))
    {
      AddError(request => request.MaintainceID, "The maintaince id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetMaintainceDTO, ComputerAssetMaintaince>(CreateEndPointUser.GetEndPointUser(User), request.MaintainceID ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetMaintainceRecord(obj.AssetDispatchID, obj.AssetRecieveID, obj.AssetState, obj.Description, obj.Diagnosis, obj.DoneBy, obj.Id, obj.Narration, obj.TreatAsExpense, obj.WhatWasDone, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
