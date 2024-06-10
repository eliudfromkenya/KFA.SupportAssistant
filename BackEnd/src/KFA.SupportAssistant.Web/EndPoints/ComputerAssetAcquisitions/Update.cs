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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

/// <summary>
/// Update an existing computer asset acquisition.
/// </summary>
/// <remarks>
/// Update an existing computer asset acquisition by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetAcquisitionRequest, UpdateComputerAssetAcquisitionResponse>
{
  private const string EndPointId = "ENP-147";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetAcquisitionRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Asset Acquisition End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Asset Acquisition";
      s.Description = "This endpoint is used to update  computer asset acquisition, making a full replacement of computer asset acquisition with a specifed valuse. A valid computer asset acquisition is required.";
      s.ExampleRequest = new UpdateComputerAssetAcquisitionRequest { AcquisitionID = "1000", AquisitionType = "Aquisition Type", AssetDetailID = "", AssetValue = 0, DateOfAcquisition = DateTime.Now, DocumentNo = "Document No", Narration = "Narration", QuotationNumber = "Quotation Number", ReceivedBy = "Received By", Value = 0, VATAmount = 0, VendorCode = "Vendor Code", WarantyEndDate = DateTime.Now };
      s.ResponseExamples[200] = new UpdateComputerAssetAcquisitionResponse(new ComputerAssetAcquisitionRecord("1000", "Aquisition Type", "", 0, DateTime.Now, "Document No", "Narration", "Quotation Number", "Received By", 0, 0, "Vendor Code", DateTime.Now, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetAcquisitionRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AcquisitionID))
    {
      AddError(request => request.AcquisitionID, "The acquisition id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetAcquisitionDTO, ComputerAssetAcquisition>(CreateEndPointUser.GetEndPointUser(User), request.AcquisitionID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer asset acquisition to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetAcquisitionDTO, ComputerAssetAcquisition>(CreateEndPointUser.GetEndPointUser(User), request.AcquisitionID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetAcquisitionResponse(new ComputerAssetAcquisitionRecord(obj.Id, obj.AquisitionType, obj.AssetDetailID, obj.AssetValue, obj.DateOfAcquisition, obj.DocumentNo, obj.Narration, obj.QuotationNumber, obj.ReceivedBy, obj.Value, obj.VATAmount, obj.VendorCode, obj.WarantyEndDate, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
