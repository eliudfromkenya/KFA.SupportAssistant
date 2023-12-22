using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetDetails;

/// <summary>
/// Create a new PurchasesBudgetDetail
/// </summary>
/// <remarks>
/// Creates a new purchases budget detail given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreatePurchasesBudgetDetailRequest, CreatePurchasesBudgetDetailResponse>
{
  private const string EndPointId = "ENP-1Q1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreatePurchasesBudgetDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Purchases Budget Detail End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new purchases budget detail";
      s.Description = "This endpoint is used to create a new  purchases budget detail. Here details of purchases budget detail to be created is provided";
      s.ExampleRequest = new CreatePurchasesBudgetDetailRequest { BatchKey = string.Empty, BuyingPrice = 0, ItemCode = "Item Code", Month = 0, Month01Quantity = 0, Month02Quantity = 0, Month03Quantity = 0, Month04Quantity = 0, Month05Quantity = 0, Month06Quantity = 0, Month07Quantity = 0, Month08Quantity = 0, Month09Quantity = 0, Month10Quantity = 0, Month11Quantity = 0, Month12Quantity = 0, Narration = "Narration", PurchasesBudgetDetailId = "1000", UnitCostPrice = 0 };
      s.ResponseExamples[200] = new CreatePurchasesBudgetDetailResponse(string.Empty, 0, "Item Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", "1000", 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreatePurchasesBudgetDetailRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<PurchasesBudgetDetailDTO>();
    requestDTO.Id = request.PurchasesBudgetDetailId;

    var result = await mediator.Send(new CreateModelCommand<PurchasesBudgetDetailDTO, PurchasesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is PurchasesBudgetDetailDTO obj)
      {
        Response = new CreatePurchasesBudgetDetailResponse(obj.BatchKey, obj.BuyingPrice, obj.ItemCode, obj.Month, obj.Month01Quantity, obj.Month02Quantity, obj.Month03Quantity, obj.Month04Quantity, obj.Month05Quantity, obj.Month06Quantity, obj.Month07Quantity, obj.Month08Quantity, obj.Month09Quantity, obj.Month10Quantity, obj.Month11Quantity, obj.Month12Quantity, obj.Narration, obj.Id, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
