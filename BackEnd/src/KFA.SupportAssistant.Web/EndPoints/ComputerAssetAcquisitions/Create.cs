using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

/// <summary>
/// Create a new ComputerAssetAcquisition
/// </summary>
/// <remarks>
/// Creates a new computer asset acquisition given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetAcquisitionRequest, CreateComputerAssetAcquisitionResponse>
{
  private const string EndPointId = "ENP-141";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetAcquisitionRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Asset Acquisition End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer asset acquisition";
      s.Description = "This endpoint is used to create a new  computer asset acquisition. Here details of computer asset acquisition to be created is provided";
      s.ExampleRequest = new CreateComputerAssetAcquisitionRequest { AcquisitionID = "1000", AquisitionType = "Aquisition Type", AssetDetailID = "", AssetValue = 0, DateOfAcquisition = DateTime.Now, DocumentNo = "Document No", Narration = "Narration", QuotationNumber = "Quotation Number", ReceivedBy = "Received By", Value = 0, VATAmount = 0, VendorCode = "Vendor Code", WarantyEndDate = DateTime.Now };
      s.ResponseExamples[200] = new CreateComputerAssetAcquisitionResponse("1000", "Aquisition Type", "", 0, DateTime.Now, "Document No", "Narration", "Quotation Number", "Received By", 0, 0, "Vendor Code", DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetAcquisitionRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetAcquisitionDTO>();
    requestDTO.Id = request.AcquisitionID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetAcquisitionDTO, ComputerAssetAcquisition>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetAcquisitionDTO obj)
      {
        Response = new CreateComputerAssetAcquisitionResponse(obj.Id, obj.AquisitionType, obj.AssetDetailID, obj.AssetValue, obj.DateOfAcquisition, obj.DocumentNo, obj.Narration, obj.QuotationNumber, obj.ReceivedBy, obj.Value, obj.VATAmount, obj.VendorCode, obj.WarantyEndDate, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
