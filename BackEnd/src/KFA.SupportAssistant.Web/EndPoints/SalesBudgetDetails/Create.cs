using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

/// <summary>
/// Create a new SalesBudgetDetail
/// </summary>
/// <remarks>
/// Creates a new sales budget detail given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateSalesBudgetDetailRequest, CreateSalesBudgetDetailResponse>
{
  private const string EndPointId = "ENP-1U1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateSalesBudgetDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Sales Budget Detail End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new sales budget detail";
      s.Description = "This endpoint is used to create a new  sales budget detail. Here details of sales budget detail to be created is provided";
      s.ExampleRequest = new CreateSalesBudgetDetailRequest { BatchKey = "Batch Key", ItemCode = "Item Code", Month = 0, Month01Quantity = 0, Month02Quantity = 0, Month03Quantity = 0, Month04Quantity = 0, Month05Quantity = 0, Month06Quantity = 0, Month07Quantity = 0, Month08Quantity = 0, Month09Quantity = 0, Month10Quantity = 0, Month11Quantity = 0, Month12Quantity = 0, Narration = "Narration", SalesBudgetDetailId = "1000", SellingPrice = 0, UnitCostPrice = 0 };
      s.ResponseExamples[200] = new CreateSalesBudgetDetailResponse("Batch Key", "Item Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", "1000", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateSalesBudgetDetailRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<SalesBudgetDetailDTO>();
    requestDTO.Id = request.SalesBudgetDetailId;

    var result = await mediator.Send(new CreateModelCommand<SalesBudgetDetailDTO, SalesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is SalesBudgetDetailDTO obj)
      {
        Response = new CreateSalesBudgetDetailResponse(obj.BatchKey, obj.ItemCode, obj.Month, obj.Month01Quantity, obj.Month02Quantity, obj.Month03Quantity, obj.Month04Quantity, obj.Month05Quantity, obj.Month06Quantity, obj.Month07Quantity, obj.Month08Quantity, obj.Month09Quantity, obj.Month10Quantity, obj.Month11Quantity, obj.Month12Quantity, obj.Narration, obj.Id, obj.SellingPrice, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
