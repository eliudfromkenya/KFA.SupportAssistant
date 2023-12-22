using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

/// <summary>
/// Create a new ExpenseBudgetBatchHeader
/// </summary>
/// <remarks>
/// Creates a new expense budget batch header given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateExpenseBudgetBatchHeaderRequest, CreateExpenseBudgetBatchHeaderResponse>
{
  private const string EndPointId = "ENP-1C1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateExpenseBudgetBatchHeaderRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Expense Budget Batch Header End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new expense budget batch header";
      s.Description = "This endpoint is used to create a new  expense budget batch header. Here details of expense budget batch header to be created is provided";
      s.ExampleRequest = new CreateExpenseBudgetBatchHeaderRequest { ApprovedBy = "Approved By", BatchKey = "1000", BatchNumber = "Batch Number", ComputerNumberOfRecords = 0, ComputerTotalAmount = 0, CostCentreCode = "Cost Centre Code", Date = DateTime.Now, MonthFrom = "Month From", MonthTo = "Month To", Narration = "Narration", NumberOfRecords = 0, PreparedBy = "Prepared By", TotalAmount = 0 };
      s.ResponseExamples[200] = new CreateExpenseBudgetBatchHeaderResponse("Approved By", "1000", "Batch Number", 0, 0, "Cost Centre Code", DateTime.Now, "Month From", "Month To", "Narration", 0, "Prepared By", 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateExpenseBudgetBatchHeaderRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ExpenseBudgetBatchHeaderDTO>();
    requestDTO.Id = request.BatchKey;

    var result = await mediator.Send(new CreateModelCommand<ExpenseBudgetBatchHeaderDTO, ExpenseBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ExpenseBudgetBatchHeaderDTO obj)
      {
        Response = new CreateExpenseBudgetBatchHeaderResponse(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.MonthFrom, obj.MonthTo, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
