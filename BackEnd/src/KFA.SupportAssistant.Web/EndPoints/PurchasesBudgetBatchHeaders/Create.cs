using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetBatchHeaders;

/// <summary>
/// Create a new PurchasesBudgetBatchHeader
/// </summary>
/// <remarks>
/// Creates a new purchases budget batch header given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreatePurchasesBudgetBatchHeaderRequest, CreatePurchasesBudgetBatchHeaderResponse>
{
  private const string EndPointId = "ENP-1P1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreatePurchasesBudgetBatchHeaderRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Purchases Budget Batch Header End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new purchases budget batch header";
      s.Description = "This endpoint is used to create a new  purchases budget batch header. Here details of purchases budget batch header to be created is provided";
      s.ExampleRequest = new CreatePurchasesBudgetBatchHeaderRequest { ApprovedBy = "Approved By", BatchKey = "1000", BatchNumber = "Batch Number", ComputerNumberOfRecords = 0, ComputerTotalAmount = 0, CostCentreCode = "Cost Centre Code", Date = DateTime.Now, MonthFrom = "Month From", MonthTo = "Month To", Narration = "Narration", NumberOfRecords = 0, PreparedBy = "Prepared By", TotalAmount = 0, TotalQuantity = 0 };
      s.ResponseExamples[200] = new CreatePurchasesBudgetBatchHeaderResponse("Approved By", "1000", "Batch Number", 0, 0, "Cost Centre Code", DateTime.Now, "Month From", "Month To", "Narration", 0, "Prepared By", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreatePurchasesBudgetBatchHeaderRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<PurchasesBudgetBatchHeaderDTO>();
    requestDTO.Id = request.BatchKey;

    var result = await mediator.Send(new CreateModelCommand<PurchasesBudgetBatchHeaderDTO, PurchasesBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is PurchasesBudgetBatchHeaderDTO obj)
      {
        Response = new CreatePurchasesBudgetBatchHeaderResponse(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.MonthFrom, obj.MonthTo, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.TotalAmount, obj.TotalQuantity, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
