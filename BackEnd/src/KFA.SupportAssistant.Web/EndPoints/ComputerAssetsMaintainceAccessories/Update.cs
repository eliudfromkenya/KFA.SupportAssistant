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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

/// <summary>
/// Update an existing computer assets maintaince accessory.
/// </summary>
/// <remarks>
/// Update an existing computer assets maintaince accessory by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetsMaintainceAccessoryRequest, UpdateComputerAssetsMaintainceAccessoryResponse>
{
  private const string EndPointId = "ENP-1B7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetsMaintainceAccessoryRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Assets Maintaince Accessory End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Assets Maintaince Accessory";
      s.Description = "This endpoint is used to update  computer assets maintaince accessory, making a full replacement of computer assets maintaince accessory with a specifed valuse. A valid computer assets maintaince accessory is required.";
      s.ExampleRequest = new UpdateComputerAssetsMaintainceAccessoryRequest { AccessoryGroupID = "", AccessoryID = "1000", Amount = 0, AssetDetailID = "", DateOfAcquisition = DateTime.Now, Description = "Description", InvoiceNumber = "Invoice Number", MaintainceID = "", Narration = "Narration", QuotationNumber = "Quotation Number", VATAmount = 0, VendorCode = "Vendor Code" };
      s.ResponseExamples[200] = new UpdateComputerAssetsMaintainceAccessoryResponse (new ComputerAssetsMaintainceAccessoryRecord("", "1000", 0, "", DateTime.Now, "Description", "Invoice Number", "", "Narration", "Quotation Number", 0, "Vendor Code", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetsMaintainceAccessoryRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AccessoryID))
    {
      AddError(request => request.AccessoryID , "The accessory id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetsMaintainceAccessoryDTO, ComputerAssetsMaintainceAccessory>(CreateEndPointUser.GetEndPointUser(User), request.AccessoryID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer assets maintaince accessory to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetsMaintainceAccessoryDTO, ComputerAssetsMaintainceAccessory>(CreateEndPointUser.GetEndPointUser(User), request.AccessoryID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetsMaintainceAccessoryResponse(new ComputerAssetsMaintainceAccessoryRecord(obj.AccessoryGroupID, obj.Id, obj.Amount, obj.AssetDetailID, obj.DateOfAcquisition, obj.Description, obj.InvoiceNumber, obj.MaintainceID, obj.Narration, obj.QuotationNumber, obj.VATAmount, obj.VendorCode, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
