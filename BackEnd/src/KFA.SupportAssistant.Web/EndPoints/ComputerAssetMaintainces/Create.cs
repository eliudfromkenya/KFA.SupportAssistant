using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

/// <summary>
/// Create a new ComputerAssetMaintaince
/// </summary>
/// <remarks>
/// Creates a new computer asset maintaince given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetMaintainceRequest, CreateComputerAssetMaintainceResponse>
{
  private const string EndPointId = "ENP-181";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetMaintainceRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Asset Maintaince End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer asset maintaince";
      s.Description = "This endpoint is used to create a new  computer asset maintaince. Here details of computer asset maintaince to be created is provided";
      s.ExampleRequest = new CreateComputerAssetMaintainceRequest { AssetDispatchID = "", AssetRecieveID = "", AssetState = "Asset State", Description = "Description", Diagnosis = "Diagnosis", DoneBy = "Done By", MaintainceID = "1000", Narration = "Narration", TreatAsExpense = true, WhatWasDone = "What Was Done" };
      s.ResponseExamples[200] = new CreateComputerAssetMaintainceResponse("", "", "Asset State", "Description", "Diagnosis", "Done By", "1000", "Narration", true, "What Was Done", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetMaintainceRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetMaintainceDTO>();
    requestDTO.Id = request.MaintainceID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetMaintainceDTO, ComputerAssetMaintaince>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);     
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetMaintainceDTO obj)
      {
        Response = new CreateComputerAssetMaintainceResponse(obj.AssetDispatchID, obj.AssetRecieveID, obj.AssetState, obj.Description, obj.Diagnosis, obj.DoneBy, obj.Id, obj.Narration, obj.TreatAsExpense, obj.WhatWasDone, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
