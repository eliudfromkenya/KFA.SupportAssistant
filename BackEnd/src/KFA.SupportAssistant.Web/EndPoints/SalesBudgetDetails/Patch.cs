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

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchSalesBudgetDetailRequest, SalesBudgetDetailRecord>
{
  private const string EndPointId = "ENP-1U6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchSalesBudgetDetailRequest.Route));
    //RequestBinder(new PatchBinder<SalesBudgetDetailDTO, SalesBudgetDetail, PatchSalesBudgetDetailRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Sales Budget Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a sales budget detail";
      s.Description = "Used to update part of an existing sales budget detail. A valid existing sales budget detail is required.";
      s.ResponseExamples[200] = new SalesBudgetDetailRecord("Batch Key", "Item Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", "1000", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchSalesBudgetDetailRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SalesBudgetDetailId))
    {
      AddError(request => request.SalesBudgetDetailId, "The sales budget detail of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    SalesBudgetDetailDTO patchFunc(SalesBudgetDetailDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<SalesBudgetDetailDTO, SalesBudgetDetail>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<SalesBudgetDetailDTO, SalesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request.SalesBudgetDetailId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the sales budget detail to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new SalesBudgetDetailRecord(obj.BatchKey, obj.ItemCode, obj.Month, obj.Month01Quantity, obj.Month02Quantity, obj.Month03Quantity, obj.Month04Quantity, obj.Month05Quantity, obj.Month06Quantity, obj.Month07Quantity, obj.Month08Quantity, obj.Month09Quantity, obj.Month10Quantity, obj.Month11Quantity, obj.Month12Quantity, obj.Narration, obj.Id, obj.SellingPrice, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
