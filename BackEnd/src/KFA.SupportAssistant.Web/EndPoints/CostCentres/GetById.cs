using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// Get a cost centre by cost centre code.
/// </summary>
/// <remarks>
/// Takes cost centre code and returns a matching cost centre record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetCostCentreByIdRequest, CostCentreRecord>
{
  private const string EndPointId = "ENP-154";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetCostCentreByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Cost Centre End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets cost centre by specified cost centre code";
      s.Description = "This endpoint is used to retrieve cost centre with the provided cost centre code";
      s.ExampleRequest = new GetCostCentreByIdRequest { CostCentreCode = "cost centre code to retrieve" };
      s.ResponseExamples[200] = new CostCentreRecord("1000", "Description", "Narration", "Region", "Supplier Code Prefix", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetCostCentreByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CostCentreCode))
    {
      AddError(request => request.CostCentreCode, "The cost centre code of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.CostCentreCode ?? "");
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
      Response = new CostCentreRecord(obj.Id, obj.Description, obj.Narration, obj.Region, obj.SupplierCodePrefix, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
