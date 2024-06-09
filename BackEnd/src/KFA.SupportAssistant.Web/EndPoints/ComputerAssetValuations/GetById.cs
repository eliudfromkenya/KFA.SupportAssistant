
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

/// <summary>
/// Get a computer asset valuation by revaluation id.
/// </summary>
/// <remarks>
/// Takes revaluation id and returns a matching computer asset valuation record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetValuationByIdRequest, ComputerAssetValuationRecord>
{
  private const string EndPointId = "ENP-1A4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetValuationByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Asset Valuation End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer asset valuation by specified revaluation id";
      s.Description = "This endpoint is used to retrieve computer asset valuation with the provided revaluation id";
      s.ExampleRequest = new GetComputerAssetValuationByIdRequest { RevaluationID = "revaluation id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetValuationRecord("", "Description", "1000", "Status", DateTime.Now, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetValuationByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RevaluationID))
    {
      AddError(request => request.RevaluationID, "The revaluation id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetValuationDTO, ComputerAssetValuation>(CreateEndPointUser.GetEndPointUser(User), request.RevaluationID ?? "");
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
      Response = new ComputerAssetValuationRecord(obj.AssetDetailID, obj.Description, obj.Id, obj.Status, obj.ValuationDate, obj.Value, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
