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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

/// <summary>
/// Update an existing computer asset valuation.
/// </summary>
/// <remarks>
/// Update an existing computer asset valuation by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetValuationRequest, UpdateComputerAssetValuationResponse>
{
  private const string EndPointId = "ENP-1A7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetValuationRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Asset Valuation End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Asset Valuation";
      s.Description = "This endpoint is used to update  computer asset valuation, making a full replacement of computer asset valuation with a specifed valuse. A valid computer asset valuation is required.";
      s.ExampleRequest = new UpdateComputerAssetValuationRequest { AssetDetailID = "", Description = "Description", RevaluationID = "1000", Status = "Status", ValuationDate = DateTime.Now, Value = 0 };
      s.ResponseExamples[200] = new UpdateComputerAssetValuationResponse(new ComputerAssetValuationRecord("", "Description", "1000", "Status", DateTime.Now, 0, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetValuationRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RevaluationID))
    {
      AddError(request => request.RevaluationID, "The revaluation id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetValuationDTO, ComputerAssetValuation>(CreateEndPointUser.GetEndPointUser(User), request.RevaluationID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer asset valuation to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetValuationDTO, ComputerAssetValuation>(CreateEndPointUser.GetEndPointUser(User), request.RevaluationID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetValuationResponse(new ComputerAssetValuationRecord(obj.AssetDetailID, obj.Description, obj.Id, obj.Status, obj.ValuationDate, obj.Value, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
