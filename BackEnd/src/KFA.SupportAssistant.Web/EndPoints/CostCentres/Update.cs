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

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// Update an existing cost centre.
/// </summary>
/// <remarks>
/// Update an existing cost centre by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateCostCentreRequest, UpdateCostCentreResponse>
{
  private const string EndPointId = "ENP-157";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateCostCentreRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Cost Centre End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Cost Centre";
      s.Description = "This endpoint is used to update  cost centre, making a full replacement of cost centre with a specifed valuse. A valid cost centre is required.";
      s.ExampleRequest = new UpdateCostCentreRequest { CostCentreCode = "1000", Description = "Description", Narration = "Narration", Region = "Region", SupplierCodePrefix = "Supplier Code Prefix" };
      s.ResponseExamples[200] = new UpdateCostCentreResponse(new CostCentreRecord("1000", "Description", "Narration", "Region", "Supplier Code Prefix", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CostCentreCode))
    {
      AddError(request => request.CostCentreCode, "The cost centre code of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.CostCentreCode ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the cost centre to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.CostCentreCode ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateCostCentreResponse(new CostCentreRecord(obj.Id, obj.Description, obj.Narration, obj.Region, obj.SupplierCodePrefix, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
