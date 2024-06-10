using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

/// <summary>
/// Get a computer asset acquisition by acquisition id.
/// </summary>
/// <remarks>
/// Takes acquisition id and returns a matching computer asset acquisition record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetAcquisitionByIdRequest, ComputerAssetAcquisitionRecord>
{
  private const string EndPointId = "ENP-144";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetAcquisitionByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Asset Acquisition End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer asset acquisition by specified acquisition id";
      s.Description = "This endpoint is used to retrieve computer asset acquisition with the provided acquisition id";
      s.ExampleRequest = new GetComputerAssetAcquisitionByIdRequest { AcquisitionID = "acquisition id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetAcquisitionRecord("1000", "Aquisition Type", "", 0, DateTime.Now, "Document No", "Narration", "Quotation Number", "Received By", 0, 0, "Vendor Code", DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetAcquisitionByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AcquisitionID))
    {
      AddError(request => request.AcquisitionID, "The acquisition id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetAcquisitionDTO, ComputerAssetAcquisition>(CreateEndPointUser.GetEndPointUser(User), request.AcquisitionID ?? "");
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
      Response = new ComputerAssetAcquisitionRecord(obj.Id, obj.AquisitionType, obj.AssetDetailID, obj.AssetValue, obj.DateOfAcquisition, obj.DocumentNo, obj.Narration, obj.QuotationNumber, obj.ReceivedBy, obj.Value, obj.VATAmount, obj.VendorCode, obj.WarantyEndDate, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
