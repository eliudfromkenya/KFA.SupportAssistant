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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetValuationRequest, ComputerAssetValuationRecord>
{
  private const string EndPointId = "ENP-1A6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetValuationRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetValuationDTO, ComputerAssetValuation, PatchComputerAssetValuationRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Asset Valuation End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer asset valuation";
      s.Description = "Used to update part of an existing computer asset valuation. A valid existing computer asset valuation is required.";
      s.ResponseExamples[200] = new ComputerAssetValuationRecord("", "Description", "1000", "Status", DateTime.Now, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetValuationRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RevaluationID))
    {
      AddError(request => request.RevaluationID , "The computer asset valuation of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetValuationDTO patchFunc(ComputerAssetValuationDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetValuationDTO, ComputerAssetValuation>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetValuationDTO, ComputerAssetValuation>(CreateEndPointUser.GetEndPointUser(User), request.RevaluationID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer asset valuation to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);      
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetValuationRecord(obj.AssetDetailID, obj.Description, obj.Id, obj.Status, obj.ValuationDate, obj.Value, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
