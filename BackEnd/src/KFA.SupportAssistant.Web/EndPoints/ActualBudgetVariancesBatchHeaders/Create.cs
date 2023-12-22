using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

/// <summary>
/// Create a new ActualBudgetVariancesBatchHeader
/// </summary>
/// <remarks>
/// Creates a new actual budget variances batch header given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateActualBudgetVariancesBatchHeaderRequest, CreateActualBudgetVariancesBatchHeaderResponse>
{
  private const string EndPointId = "ENP-111";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateActualBudgetVariancesBatchHeaderRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Actual Budget Variances Batch Header End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new actual budget variances batch header";
      s.Description = "This endpoint is used to create a new  actual budget variances batch header. Here details of actual budget variances batch header to be created is provided";
      s.ExampleRequest = new CreateActualBudgetVariancesBatchHeaderRequest { ApprovedBy = "Approved By", BatchKey = "1000", BatchNumber = "Batch Number", CashSalesAmount = 0, ComputerNumberOfRecords = 0, ComputerTotalActualAmount = 0, ComputerTotalBudgetAmount = 0, CostCentreCode = "Cost Centre Code", Month = "Month", Narration = "Narration", NumberOfRecords = 0, PreparedBy = "Prepared By", PurchasesesAmount = 0, TotalActualAmount = 0, TotalBudgetAmount = 0 };
      s.ResponseExamples[200] = new CreateActualBudgetVariancesBatchHeaderResponse("Approved By", "1000", "Batch Number", 0, 0, 0, 0, "Cost Centre Code", "Month", "Narration", 0, "Prepared By", 0, 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateActualBudgetVariancesBatchHeaderRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ActualBudgetVariancesBatchHeaderDTO>();
    requestDTO.Id = request.BatchKey;

    var result = await mediator.Send(new CreateModelCommand<ActualBudgetVariancesBatchHeaderDTO, ActualBudgetVariancesBatchHeader>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ActualBudgetVariancesBatchHeaderDTO obj)
      {
        Response = new CreateActualBudgetVariancesBatchHeaderResponse(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.CashSalesAmount, obj.ComputerNumberOfRecords, obj.ComputerTotalActualAmount, obj.ComputerTotalBudgetAmount, obj.CostCentreCode, obj.Month, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.PurchasesesAmount, obj.TotalActualAmount, obj.TotalBudgetAmount, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
