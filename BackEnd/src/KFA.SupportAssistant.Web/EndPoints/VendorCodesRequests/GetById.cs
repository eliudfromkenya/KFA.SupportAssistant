using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

/// <summary>
/// Get a vendor codes request by vendor code request id.
/// </summary>
/// <remarks>
/// Takes vendor code request id and returns a matching vendor codes request record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetVendorCodesRequestByIdRequest, VendorCodesRequestRecord>
{
  private const string EndPointId = "ENP-274";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetVendorCodesRequestByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Vendor Codes Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets vendor codes request by specified vendor code request id";
      s.Description = "This endpoint is used to retrieve vendor codes request with the provided vendor code request id";
      s.ExampleRequest = new GetVendorCodesRequestByIdRequest { VendorCodeRequestID = "vendor code request id to retrieve" };
      s.ResponseExamples[200] = new VendorCodesRequestRecord("Attanded By", "Cost Centre Code", "Description", "Narration", "Requesting User", "Status", DateTime.Now, DateTime.Now, "Vendor Code", "1000", "Vendor Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetVendorCodesRequestByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VendorCodeRequestID))
    {
      AddError(request => request.VendorCodeRequestID, "The vendor code request id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<VendorCodesRequestDTO, VendorCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.VendorCodeRequestID ?? "");
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
      Response = new VendorCodesRequestRecord(obj.AttandedBy, obj.CostCentreCode, obj.Description, obj.Narration, obj.RequestingUser, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.VendorCode, obj.Id, obj.VendorType, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
