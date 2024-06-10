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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetAcquisitionRequest, ComputerAssetAcquisitionRecord>
{
  private const string EndPointId = "ENP-146";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetAcquisitionRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetAcquisitionDTO, ComputerAssetAcquisition, PatchComputerAssetAcquisitionRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Asset Acquisition End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer asset acquisition";
      s.Description = "Used to update part of an existing computer asset acquisition. A valid existing computer asset acquisition is required.";
      s.ResponseExamples[200] = new ComputerAssetAcquisitionRecord("1000", "Aquisition Type", "", 0, DateTime.Now, "Document No", "Narration", "Quotation Number", "Received By", 0, 0, "Vendor Code", DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetAcquisitionRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AcquisitionID))
    {
      AddError(request => request.AcquisitionID, "The computer asset acquisition of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetAcquisitionDTO patchFunc(ComputerAssetAcquisitionDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetAcquisitionDTO, ComputerAssetAcquisition>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetAcquisitionDTO, ComputerAssetAcquisition>(CreateEndPointUser.GetEndPointUser(User), request.AcquisitionID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer asset acquisition to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetAcquisitionRecord(obj.Id, obj.AquisitionType, obj.AssetDetailID, obj.AssetValue, obj.DateOfAcquisition, obj.DocumentNo, obj.Narration, obj.QuotationNumber, obj.ReceivedBy, obj.Value, obj.VATAmount, obj.VendorCode, obj.WarantyEndDate, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
