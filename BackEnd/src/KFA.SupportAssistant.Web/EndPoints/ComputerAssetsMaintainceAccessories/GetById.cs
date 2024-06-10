using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

/// <summary>
/// Get a computer assets maintaince accessory by accessory id.
/// </summary>
/// <remarks>
/// Takes accessory id and returns a matching computer assets maintaince accessory record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetsMaintainceAccessoryByIdRequest, ComputerAssetsMaintainceAccessoryRecord>
{
  private const string EndPointId = "ENP-1B4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetsMaintainceAccessoryByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Assets Maintaince Accessory End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer assets maintaince accessory by specified accessory id";
      s.Description = "This endpoint is used to retrieve computer assets maintaince accessory with the provided accessory id";
      s.ExampleRequest = new GetComputerAssetsMaintainceAccessoryByIdRequest { AccessoryID = "accessory id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetsMaintainceAccessoryRecord("", "1000", 0, "", DateTime.Now, "Description", "Invoice Number", "", "Narration", "Quotation Number", 0, "Vendor Code", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetsMaintainceAccessoryByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AccessoryID))
    {
      AddError(request => request.AccessoryID, "The accessory id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetsMaintainceAccessoryDTO, ComputerAssetsMaintainceAccessory>(CreateEndPointUser.GetEndPointUser(User), request.AccessoryID ?? "");
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
      Response = new ComputerAssetsMaintainceAccessoryRecord(obj.AccessoryGroupID, obj.Id, obj.Amount, obj.AssetDetailID, obj.DateOfAcquisition, obj.Description, obj.InvoiceNumber, obj.MaintainceID, obj.Narration, obj.QuotationNumber, obj.VATAmount, obj.VendorCode, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
