using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

/// <summary>
/// Create a new ComputerAssetsMaintainceAccessory
/// </summary>
/// <remarks>
/// Creates a new computer assets maintaince accessory given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetsMaintainceAccessoryRequest, CreateComputerAssetsMaintainceAccessoryResponse>
{
  private const string EndPointId = "ENP-1B1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetsMaintainceAccessoryRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Assets Maintaince Accessory End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer assets maintaince accessory";
      s.Description = "This endpoint is used to create a new  computer assets maintaince accessory. Here details of computer assets maintaince accessory to be created is provided";
      s.ExampleRequest = new CreateComputerAssetsMaintainceAccessoryRequest { AccessoryGroupID = "", AccessoryID = "1000", Amount = 0, AssetDetailID = "", DateOfAcquisition = DateTime.Now, Description = "Description", InvoiceNumber = "Invoice Number", MaintainceID = "", Narration = "Narration", QuotationNumber = "Quotation Number", VATAmount = 0, VendorCode = "Vendor Code" };
      s.ResponseExamples[200] = new CreateComputerAssetsMaintainceAccessoryResponse("", "1000", 0, "", DateTime.Now, "Description", "Invoice Number", "", "Narration", "Quotation Number", 0, "Vendor Code", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetsMaintainceAccessoryRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetsMaintainceAccessoryDTO>();
    requestDTO.Id = request.AccessoryID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetsMaintainceAccessoryDTO, ComputerAssetsMaintainceAccessory>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetsMaintainceAccessoryDTO obj)
      {
        Response = new CreateComputerAssetsMaintainceAccessoryResponse(obj.AccessoryGroupID, obj.Id, obj.Amount, obj.AssetDetailID, obj.DateOfAcquisition, obj.Description, obj.InvoiceNumber, obj.MaintainceID, obj.Narration, obj.QuotationNumber, obj.VATAmount, obj.VendorCode, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
